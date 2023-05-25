using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface IBase
    {
        void Salvar();
        void Excluir();
        int Key { get; }
        void CriarTabela();
        List<IBase> Todos();
        List<IBase> Buscar();
    }
}
