using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRestCommunication
{
    public class NotificarErro 
    {
        private List<string> Erros { get; set; }

        public NotificarErro()
        {
            Erros = new List<string>();
        }

        public void LimparErros()
        {
            Erros.Clear();
        }

        public void AdicionarErro(string erro)
        {
            Erros.Add(erro);
        }

        public void AdicionarErro(IEnumerable<string> erros)
        {
            Erros.AddRange(erros);
        }

        public void AdicionarErro(Exception ex)
        {
            Erros.Add(ex.Message);
        }

        public void AdicionarErro(IEnumerable<Exception> exs)
        {
            Erros.AddRange(exs.Select(x => x.Message));
        }

        public bool Valido()
        {
            return !Erros.Any();
        }

        public IEnumerable<string> TodosErros()
        {
            return Erros;
        }
    }
}
