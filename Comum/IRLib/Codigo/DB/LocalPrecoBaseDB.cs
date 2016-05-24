using CTLib;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib
{

    #region "LocalPrecoBase_B"
    public abstract class LocalPrecoBase_B : BaseBD
    {
        public localid LocalID = new localid();
        public nome Nome = new nome();
        public desconto Desconto = new desconto();
        public principal Principal = new principal();

        public LocalPrecoBase_B() { }

        // passar o Usuario logado no sistema
        public LocalPrecoBase_B(int usuarioIDLogado)
        {
            this.Control.UsuarioID = usuarioIDLogado;
        }

        /// <summary>
        /// Preenche todos os atributos de Preco
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override void Ler(int id)
        {

            try
            {
                string sql = "SELECT * FROM LocalPrecoBase WHERE ID = " + id;
                using (bd.Consulta(sql))
                {
                    if (bd.Consulta().Read())
                    {
                        this.Control.ID = id;
                        this.LocalID.ValorBD = bd.LerInt("LocalID").ToString();
                        this.Nome.ValorBD = bd.LerString("Nome");
                        this.Desconto.ValorBD = bd.LerDecimal("Desconto").ToString();
                        this.Principal.ValorBD = bd.LerBoolean("Principal").ToString();
                    }
                    else
                        this.Limpar();

                    bd.Fechar();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Inserir novo(a) Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir()
        {
            try
            {
                return Inserir(bd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Inserir novo(a) Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Inserir(BD bd)
        {
            try
            {
                string query = @"INSERT INTO LocalPrecoBase
                                       (LocalID
                                       ,Nome
                                       ,Desconto
                                       ,Principal)
                                        OUTPUT INSERTED.ID
                                 VALUES
                                       (@LocalID
                                       ,'@Nome'
                                       ,@Desconto
                                       ,'@Principal')";
                query = query.Replace("@LocalID", this.LocalID.ValorBD);
                query = query.Replace("@Nome", this.Nome.ValorBD);
                query = query.Replace("@Desconto", this.Desconto.ValorBD);
                query = query.Replace("@Principal", this.Principal.Valor.ToString());
                this.Control.ID = bd.ExecutarScalar(query);
                return this.Control.ID > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Atualiza Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar()
        {
            try
            {
                return Atualizar(bd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Atualiza Preco
        /// </summary>
        /// <returns></returns>	
        public override bool Atualizar(BD bd)
        {
            try
            {
                string query = @"UPDATE LocalPrecoBase
                                SET LocalID = @LocalID
                                    ,Nome = '@Nome'
                                    ,Desconto = @Desconto
                                    ,Principal = '@Principal'
                                WHERE
                                    ID = @ID";
                query = query.Replace("@ID", this.Control.ID.ToString());
                query = query.Replace("@LocalID", this.LocalID.ValorBD);
                query = query.Replace("@Nome", this.Nome.ValorBD);
                query = query.Replace("@Desconto", this.Desconto.ValorBD);
                query = query.Replace("@Principal", this.Principal.Valor.ToString());
                return bd.Executar(query) == 1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Exclui Preco
        /// </summary>
        /// <returns></returns>		
        public override bool Excluir()
        {
            try
            {
                return this.Excluir(this.Control.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exclui Preco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(int id)
        {
            try
            {
                return Excluir(bd, id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exclui Preco com ID especifico
        /// </summary>
        /// <param name="id">Informe o ID</param>
        /// <returns></returns>
        public override bool Excluir(BD bd, int id)
        {
            try
            {
                this.Control.ID = id;
                string sqlDelete = "DELETE FROM LocalPrecoBase WHERE ID = " + id;
                return bd.Executar(sqlDelete) == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public override void Limpar()
        {
            this.Nome.Limpar();
            this.LocalID.Limpar();
            this.Desconto.Limpar();
            this.Principal.Limpar();
        }

        public override void Desfazer()
        {
            this.Nome.Desfazer();
            this.LocalID.Desfazer();
            this.Desconto.Desfazer();
            this.Principal.Desfazer();
        }


        public class nome : TextProperty
        {

            public override string Nome
            {
                get
                {
                    return "Nome";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 30;
                }
            }

            public override string Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor;
            }

        }
        public class localid : IntegerProperty
        {

            public override string Nome
            {
                get
                {
                    return "LocalID";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override int Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }
        public class principal : BooleanProperty
        {

            public override string Nome
            {
                get
                {
                    return "Principal";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override bool Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }
        public class desconto : NumberProperty
        {

            public override string Nome
            {
                get
                {
                    return "Desconto";
                }
            }

            public override int Tamanho
            {
                get
                {
                    return 0;
                }
            }

            public override decimal Valor
            {
                get
                {
                    return base.Valor;
                }
                set
                {
                    base.Valor = value;
                }
            }

            public override string ToString()
            {
                return base.Valor.ToString();
            }

        }
    }
    #endregion
}