using CTLib;
using System;
using System.Linq;

namespace IRLib.Paralela.Assinaturas
{
    public class ImportacaoCultura
    {
        public void MudarClientes()
        {
            BD bd = new BD();
            BD bdAux = new BD();
            try
            {
                var obj = new
                {
                    UserID = 0,
                    ClienteID = 0,
                    NovoClienteID = 0,
                    CPF = string.Empty,
                };


                var listaAnonima = VendaBilheteria.ToAnonymousList(obj);

                string sql =
                    @"
                        SELECT UserID, ClienteID, ClienteIDBase, CPFBase FROM ClientesCultura WHERE ClienteID <> ClienteIDBase AND CPFBase IS NOT NULL AND Importado = 0
                    ";

                bd.Consulta(sql);

                if (!bd.Consulta().Read())
                    throw new Exception("No more entries!");

                do
                {
                    listaAnonima.Add(new
                    {
                        UserID = bd.LerInt("UserID"),
                        ClienteID = bd.LerInt("ClienteID"),
                        NovoClienteID = bd.LerInt("ClienteIDBase"),
                        CPF = bd.LerString("CPFBase")
                    });
                } while (bd.Consulta().Read());
                bd.FecharConsulta();

                foreach (var cliente in listaAnonima)
                {
                    try
                    {
                        bd.IniciarTransacao();

                        //Tem o CPF na base CC mas o cliente não existe e foi criado um novo, só altera o CPF dele.
                        if (cliente.NovoClienteID == 0)
                        {
                            bd.Executar("UPDATE tCliente SET CPF = '" + cliente.CPF + "' WHERE ID = " + cliente.ClienteID);
                            bd.Executar("UPDATE ClientesCultura SET Importado = 1 WHERE UserID = " + cliente.UserID);
                            bd.FinalizarTransacao();
                            continue;
                        }


                        var objAssinatura = new
                        {
                            AssinaturaClienteID = 0,
                            VendaBilheteriaID = 0,
                        };

                        var listaAssinaturaAnonima = VendaBilheteria.ToAnonymousList(objAssinatura);

                        bdAux.Consulta("SELECT ac.ID, ac.VendaBilheteriaID FROM tAssinaturaCliente ac (NOLOCK) INNER JOIN tAssinatura a (NOLOCK) ON a.ID = ac.AssinaturaID WHERE ClienteID = " + cliente.ClienteID + " AND a.AssinaturaTipoID = 3");

                        if (!bdAux.Consulta().Read())
                            throw new Exception("Não existem assinaturas para este cliente.");

                        do
                        {
                            listaAssinaturaAnonima.Add(new
                            {
                                AssinaturaClienteID = bdAux.LerInt("ID"),
                                VendaBilheteriaID = bdAux.LerInt("VendaBilheteriaID"),
                            });
                        } while (bdAux.Consulta().Read());

                        foreach (var assinatura in listaAssinaturaAnonima)
                        {
                            bd.Executar(string.Format("UPDATE tAssinaturaCliente SET ClienteID = {0} WHERE ID = {1}", cliente.NovoClienteID, assinatura.AssinaturaClienteID));
                            bd.Executar(string.Format("UPDATE tIngresso SET ClienteID = {0} WHERE AssinaturaClienteID = {1} AND Status IN ('B', 'V')", cliente.NovoClienteID, assinatura.AssinaturaClienteID));
                        }

                        if (listaAssinaturaAnonima.Where(c => c.VendaBilheteriaID > 0).Count() > 0)
                            if (bd.Executar("UPDATE tVendaBilheteria SET ClienteID = " + cliente.NovoClienteID + " WHERE ID " + listaAssinaturaAnonima.Where(c => c.VendaBilheteriaID > 0).FirstOrDefault()) != 1)
                                throw new Exception("Existe registro de Venda mas não foi possível alterar o registro");

                        bd.Executar("UPDATE ClientesCultura SET Importado = 1 WHERE UserID = " + cliente.UserID);

                        bd.FinalizarTransacao();
                    }
                    catch (Exception ex)
                    {
                        bd.DesfazerTransacao();
                        bd.Executar("UPDATE ClientesCultura SET Erro = '" + ex.Message + "' WHERE UserID = " + cliente.UserID);
                    }
                    finally
                    {
                        bd.FecharConsulta();
                        bdAux.FecharConsulta();
                    }
                }

            }
            finally
            {
                bd.Fechar();
            }
        }
    }
}
