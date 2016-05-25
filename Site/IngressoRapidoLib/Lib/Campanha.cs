using System;
using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public class Campanha
    {
        private int id;
        private string nome;
        private int pontos;
        private int autor;

        private List<CampanhaTipo> campanhaTipo = new List<CampanhaTipo>();
        private DateTime timestamp;

        public List<CampanhaTipo> CampanhaTipo
        {
            get { return campanhaTipo; }
            set { campanhaTipo = value; }
        }

        public string Nome
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string Autor
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Pontos
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void IncluirCampanha()
        {
            throw new System.NotImplementedException();
        }

        public void ConsultarCampanha(int nome)
        {
            throw new System.NotImplementedException();
        }

        public void ExcluirCampanha()
        {
            throw new System.NotImplementedException();
        }

        public void GetbyId()
        { }
    }
}

