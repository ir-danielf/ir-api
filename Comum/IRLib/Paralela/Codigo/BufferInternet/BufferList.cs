using CTLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{
    /// <summary>
    /// Lista Generica utilizada no Novo Johnny Five
    ///     Construtor Com parametros
    ///         Deve ser passado o ID (IR_xxID Ou só ID (Depende da Tabela) que será utilizado para o join do Delete.., Nome da Tabela no Enumerador, Bool informando
    ///         se existe update (Tabelas de N - N nao tem, exceto EventoTaxaEntrega
    ///     Construtor Sem Parametros
    ///         Sua utilizacao é limitada a armazenar os dados
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BufferList<T> : List<T> where T : aSync<T>
    {
        #region Fields, Properties
        private DAL oDAL = new DAL();
        private bool hasUpdate { get; set; }
        private BufferInternet.enumTables Table { get; set; }
        private string Field { get; set; }

        public BufferList<T> lstSite { get; set; }

        private List<T> ItemsToInsert { get; set; }
        private List<T> ItemsToUpdate { get; set; }
        private List<int> ItemsToDelete { get; set; }

        bool SaveAllLogs { get; set; }
        bool Error = false;
        StringBuilder ErrorMessage = new StringBuilder();
        DateTime StartTime;

        int errorDelete = 0;
        int errorInsert = 0;
        int errorUpdate = 0;

        #endregion

        #region Construtores
        public BufferList(string field, BufferInternet.enumTables table, bool hasupdate, bool saveAllLogs)
        {
            this.Field = field;
            this.Table = table;
            this.hasUpdate = hasupdate;
            this.SaveAllLogs = saveAllLogs;
        }
        public BufferList()
        { }
        #endregion

        #region Sincronizacao de Tabelas Genericas
        public void Syncronize()
        {
            try
            {
                StartTime = DateTime.Now;
                oDAL.ConnOpen();

                this.GetItemsToInsert();

                if (hasUpdate)
                    this.GetItemsToUpdate();

                this.GetItemsToDelete();


            }
            catch (Exception ex)
            {
                this.Error = true;
                ErrorMessage.Append(ex.Message);
                throw ex;
            }
            finally
            {
                if (SaveAllLogs || Error || this.errorDelete > 0 || this.errorUpdate > 0 || this.errorInsert > 0)
                {
                    if ((this.errorDelete > 0 || this.errorUpdate > 0 || this.errorInsert > 0) && ErrorMessage.Length == 0)
                    {
                        this.Error = true;
                        this.ErrorMessage.Append("\nOcorreram Erros ao executar as ações de U/I/D");

                        if (this.errorDelete > 0)
                            this.ErrorMessage.Append("\nDelete: " + errorDelete);

                        if (this.errorUpdate > 0)
                            this.ErrorMessage.Append("\nUpdate: " + errorUpdate);

                        if (this.errorInsert > 0)
                            this.ErrorMessage.Append("\nInsert:" + errorInsert);
                    }
                    this.SaveLog();
                }
                oDAL.ConnClose();
            }
        }

        private void GetItemsToUpdate()
        {

            this.ItemsToUpdate = (from sistema in this
                                  join site in lstSite on sistema.ID equals site.ID
                                  where sistema.ID > 0 && site.ID > 0 && !sistema.CompareIt(site)
                                  select sistema).ToList();

            foreach (T item in this.ItemsToUpdate)
            {
                if (oDAL.Execute(item.UpdateSQL, item.setParameters().ToArray()) > 0)
                {
                    lstSite.RemoveAll(c => c.ID == item.ID);
                    lstSite.Add(item);
                }
                else errorUpdate++;
            }
        }

        private void GetItemsToInsert()
        {
            this.ItemsToInsert = (from sistema in this
                                  where !lstSite.Any(c => c.ID.Equals(sistema.ID) && sistema.ID > 0)
                                  select sistema).ToList();

            foreach (T item in this.ItemsToInsert)
            {
                if (oDAL.Execute(item.InsertSQL, item.setParameters().ToArray()) > 0)
                    this.lstSite.Add(item);
                else
                    errorInsert++;
            }
        }

        private void GetItemsToDelete()
        {
            DataTable dttItemsToDelete = new DataTable(typeof(T).ToString());
            dttItemsToDelete.Columns.Add("ID", typeof(int));

            this.ItemsToDelete = (from site in lstSite
                                  where !this.Any(c => c.ID.Equals(site.ID) && site.ID > 0 && c.ID > 0)
                                  select site.ID).ToList();

            DataRow row;
            ItemsToDelete.ForEach(delegate(int ID)
            {
                row = dttItemsToDelete.NewRow();
                row["ID"] = ID;
                dttItemsToDelete.Rows.Add(ID);
                lstSite.RemoveAll(c => c.ID == ID);
            }
            );

            if (dttItemsToDelete.Rows.Count > 0)
            {
                oDAL.BulkInsert(dttItemsToDelete, "Robo_" + Table + "Excluir", true);
                errorDelete = dttItemsToDelete.Rows.Count - this.ApagarRegistroInexistente();
            }
        }
        #endregion

        #region Sincronizacao de Tabelas Especiais

        #region Setor
        public void SyncronizeSetor()
        {
            try
            {
                oDAL.ConnOpen();

                this.GetItemsToInsertSetor();
                this.GetItemsToUpdateSetor();
                this.GetItemsToDeleteSetor();
            }
            catch (Exception ex)
            {
                Error = true;
                throw ex;
            }
            finally
            {
                if (SaveAllLogs || Error || this.errorDelete > 0 || this.errorUpdate > 0 || this.errorInsert > 0)
                {
                    if ((this.errorDelete > 0 || this.errorUpdate > 0 || this.errorInsert > 0) && ErrorMessage.Length == 0)
                    {
                        this.Error = true;
                        this.ErrorMessage.Append("\nOcorreram Erros ao executar as ações de U/I/D");

                        if (this.errorDelete > 0)
                            this.ErrorMessage.Append("\nDelete: " + errorDelete);

                        if (this.errorUpdate > 0)
                            this.ErrorMessage.Append("\nUpdate: " + errorUpdate);

                        if (this.errorInsert > 0)
                            this.ErrorMessage.Append("\nInsert:" + errorInsert);
                    }
                    this.SaveLog();
                }
                oDAL.ConnClose();
            }
        }

        private void GetItemsToInsertSetor()
        {
            this.ItemsToInsert = (from sistema in this
                                  where !this.lstSite.Any(c => c.ID.Equals(sistema.ID) && c.ApresentacaoID.Equals(sistema.ApresentacaoID))
                                  select sistema).ToList();


            foreach (T item in this.ItemsToInsert)
            {
                if (oDAL.Execute(item.InsertSQL, item.setParameters().ToArray()) > 0)
                    this.lstSite.Add(item);
                else
                    errorInsert++;
            }

        }

        private void GetItemsToUpdateSetor()
        {

            this.ItemsToUpdate = (from sistema in this
                                  join site in this.lstSite on sistema.ID equals site.ID
                                  where site.ApresentacaoID == sistema.ApresentacaoID && !sistema.CompareIt(site)
                                  select sistema).ToList();

            //this.ItemsToUpdate = (from site in this.lstSite
            //                      join sistema in this on new { site.ID, site.ApresentacaoID } equals new { sistema.ID, sistema.ApresentacaoID }
            //                      where !sistema.CompareIt(site)
            //                      select sistema).ToList();

            foreach (T item in ItemsToUpdate)
            {
                if (oDAL.Execute(item.UpdateSQL, item.setParameters().ToArray()) > 0)
                {
                    this.lstSite.RemoveAll(c => c.ID == item.ID && c.ApresentacaoID == item.ApresentacaoID);
                    this.lstSite.Add(item);
                }
                else
                    errorUpdate++;
            }

        }

        private void GetItemsToDeleteSetor()
        {
            List<T> SetoresToDelete = (from site in this.lstSite
                                       where !this.Any(c => c.ID.Equals(site.ID) && c.ApresentacaoID.Equals(site.ApresentacaoID))
                                       select site).ToList();

            foreach (T item in SetoresToDelete)
            {
                if (oDAL.Execute("DELETE FROM Setor WHERE IR_SetorID = " + item.ID + " AND ApresentacaoID =" + item.ApresentacaoID) > 0)
                    this.lstSite.RemoveAll(c => c.ID == item.ID && c.ApresentacaoID == item.ApresentacaoID);
                else
                    errorDelete++;
            }
        }

        #endregion

        #endregion

        private int ApagarRegistroInexistente()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();

                stbSQL.Append("DELETE localTable FROM " + this.Table + " AS localTable ");
                stbSQL.Append("INNER JOIN Robo_" + this.Table + "Excluir deleteTable ON localTable." + this.Field + " = deleteTable.ID ");
                return oDAL.Execute(stbSQL.ToString());
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao excluir dados da tabela " + this.Table + ": " + ex.Message);
            }
        }

        private void SaveLog()
        {
            try
            {
                EventLog log = new EventLog("Robot Johnny Five");

                log.Source = "Robot Johnny Five";
                log.Log = "Atualização SiteIR";
                if (this.Error)
                    log.WriteEntry("Atualização da tabela \"" + this.Table + "\" Status: Erro, Mensagem: " + this.ErrorMessage + " em: " + (DateTime.Now.Subtract(this.StartTime).Seconds) + " segundos", EventLogEntryType.FailureAudit, 666);
                else
                    if (SaveAllLogs)
                        log.WriteEntry("Atualização da tabela \"" + this.Table + "\" Status: Sucesso em: " + (DateTime.Now.Subtract(this.StartTime).Seconds) + " segundos", EventLogEntryType.Information, 3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
