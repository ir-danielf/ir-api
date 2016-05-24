using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace IRLib.Codigo.Sftp
{
    public class GeraEnviaArquivo
    {
        public static string GeraArquivo()
        {
            string retornoNomeArquivo = string.Empty;

            string diretorio = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            List<RelatorioClientesCotaNet> dados = getInfosRelatorio(DateTime.Now.AddMonths(-1).ToString("yyyyMM"));

            StringBuilder csv = new StringBuilder();
            csv.Append("CPF do Responsável pela compra;Nome do Responsável pela compra;Local;Evento;Data Apresentação;Hora Apresentação;Setor;Data Compra;Valor Ticket;Quantidade Ticket;Flag se o CPF é NET" + (dados.Count() > 0 ? Environment.NewLine : ""));
            for (int i = 0; i < dados.Count(); i++)
            {
                string newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}{11}",
                    dados[i].CPFResponsavelCompra, dados[i].NomeResponsavelCompra, dados[i].Local, dados[i].Evento,
                    dados[i].DataApresentacao, dados[i].HoraApresentacao, dados[i].Setor, dados[i].DataCompra,
                    dados[i].ValorTicket, dados[i].QuantidadeTicket,
                    dados[i].FlagClienteNet, (i != (dados.Count() - 1) ? Environment.NewLine : ""));
                csv.Append(newLine);
            }
            byte[] bytes = new byte[csv.ToString().Length * sizeof(char)];
            System.Buffer.BlockCopy(csv.ToString().ToCharArray(), 0, bytes, 0, bytes.Length);

            retornoNomeArquivo = diretorio + "\\ClientesCotaNet " + DateTime.Now.ToString("dd-MM-yyyy") + ".csv";
            var bw = new BinaryWriter(File.Open(retornoNomeArquivo, FileMode.OpenOrCreate));
            bw.Write(bytes);
            bw.Flush();
            bw.Close();
            //return File(bytes, "text/csv", "ClientesCotaNet " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".csv");
            return retornoNomeArquivo;
        }
        public static void Upload(string arquivo)
        {

            // informações de acesso ao sftp
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = ConfigurationManager.AppSettings["HostName"],
                UserName = ConfigurationManager.AppSettings["UserName"],
                Password = ConfigurationManager.AppSettings["Password"],
                PortNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]),
                SshHostKeyFingerprint = ConfigurationManager.AppSettings["SshHostKeyFingerprint"]
            };
            using (Session session = new Session())
            {
                session.SessionLogPath = null;
                session.Open(sessionOptions); //Attempts to connect to your sFtp site
                //Get Ftp File
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary; //The Transfer Mode - 
                //<em style="font-size: 9pt;">Automatic, Binary, or Ascii  
                transferOptions.FilePermissions = null; //Permissions applied to remote files; 
                //null for default permissions.  Can set user, 
                //Group, or other Read/Write/Execute permissions. 
                transferOptions.PreserveTimestamp = false; //Set last write time of 
                //destination file to that of source file - basically change the timestamp 
                //to match destination and source files.   
                transferOptions.ResumeSupport.State = TransferResumeSupportState.Off;

                TransferOperationResult transferResult;
                //the parameter list is: local Path, Remote Path, Delete source file?, transfer Options  
                transferResult = session.PutFiles(arquivo, ConfigurationManager.AppSettings["DiretorioDestino"], true, transferOptions);
                //Throw on any error 
                transferResult.Check();
                //Log information and break out if necessary  

            }
        }
        private static List<RelatorioClientesCotaNet> getInfosRelatorio(string DataFiltro)
        {
            DateTime dt = System.DateTime.Now;
            BD bd = new BD();

            StringBuilder str = new StringBuilder();
            str.AppendFormat(@"SELECT 
	                            c.CPF AS CPFResponsavelCompra
	                            ,c.Nome AS NomeResponsavelCompra
	                            ,l.Nome AS Local
	                            ,e.Nome AS Evento
	                            ,(SUBSTRING(a.Horario,7,2) + '/' + SUBSTRING(a.Horario,5,2) + '/' + SUBSTRING(a.Horario,1,4)) AS DataApresentacao
	                            ,(SUBSTRING(a.Horario,9,2) + ':' + SUBSTRING(a.Horario,11,2) + ':' + SUBSTRING(a.Horario,13,2)) AS HoraApresentacao
	                            ,s.Nome AS Setor
	                            ,(SUBSTRING(vb.DataVenda,7,2) + '/' + SUBSTRING(vb.DataVenda,5,2) + '/' + SUBSTRING(vb.DataVenda,1,4) + ' ' + SUBSTRING(vb.DataVenda,9,2) + ':' + SUBSTRING(vb.DataVenda,11,2) + ':' + SUBSTRING(vb.DataVenda,13,2)) AS DataCompra 
	                            ,p.Valor AS ValorTicket
	                            ,COUNT(*) AS QuantidadeTicket
	                            ,CASE 
		                            WHEN c.CPF COLLATE Latin1_General_CI_AI = ic.CodigoPromocional COLLATE Latin1_General_CI_AI 
			                            THEN 'Sim' 
			                            ELSE 'Não'
		                            END AS FlagClienteNet
                            FROM 
	                            tIngresso i(NOLOCK) 
	                            LEFT JOIN tVendaBilheteria vb (NOLOCK) on vb.ID = i.VendaBilheteriaID
	                            INNER JOIN tIngressoCliente ic (NOLOCK) on ic.IngressoID = i.ID
	                            INNER JOIN tCliente c (NOLOCK) on c.ID = i.ClienteID
	                            INNER JOIN tCotaItem cti (NOLOCK) on cti.ID = ic.CotaItemID
	                            INNER JOIN tLocal l (NOLOCK) on l.ID = i.LocalID	
	                            INNER JOIN tEvento e (NOLOCK) on e.ID = i.EventoID
	                            INNER JOIN tApresentacao a (NOLOCK) on a.ID = i.ApresentacaoID
	                            INNER JOIN tSetor s (NOLOCK) on s.ID = i.SetorID
	                            INNER JOIN tPreco p (NOLOCK) on p.ID = i.PrecoID
                            WHERE 
	                            cti.ParceiroID = 786
	                            AND vb.DataVenda LIKE '{0}%'
                            GROUP BY 
	                            c.CPF
	                            ,c.Nome
	                            ,l.Nome
	                            ,e.Nome
	                            ,a.Horario
	                            ,s.Nome
	                            ,vb.DataVenda
	                            ,p.Valor
	                            ,ic.CodigoPromocional
                            ORDER BY 
	                            l.Nome,e.Nome,a.Horario,s.Nome", DataFiltro);

            List<RelatorioClientesCotaNet> listRelatorio = new List<RelatorioClientesCotaNet>();

            bd.Consulta(str.ToString());

            while (bd.Consulta().Read())
            {
                RelatorioClientesCotaNet rcc = new RelatorioClientesCotaNet();

                rcc.CPFResponsavelCompra = bd.LerString("CPFResponsavelCompra");
                rcc.NomeResponsavelCompra = bd.LerString("NomeResponsavelCompra");
                rcc.Local = bd.LerString("Local");
                rcc.Evento = bd.LerString("Evento");
                rcc.DataApresentacao = bd.LerString("DataApresentacao");
                rcc.HoraApresentacao = bd.LerString("HoraApresentacao");
                rcc.Setor = bd.LerString("Setor");
                rcc.DataCompra = bd.LerString("DataCompra");
                rcc.ValorTicket = bd.LerString("ValorTicket");
                rcc.QuantidadeTicket = bd.LerInt("QuantidadeTicket");
                rcc.FlagClienteNet = bd.LerString("FlagClienteNet");

                listRelatorio.Add(rcc);
            }

            return listRelatorio;
        }
    }
}
