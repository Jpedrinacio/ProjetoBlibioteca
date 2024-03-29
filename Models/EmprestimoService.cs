using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;

                bc.SaveChanges();
            }
        }

        public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos filtro)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Emprestimo>query;
                if (filtro != null)
                {
                     //definindo dinamicamente a filtragem
                    switch(filtro.TipoFiltro)
                    {
                        case "Usuario":
                            query = bc.Emprestimos.Where(l => l.NomeUsuario.Contains(filtro.Filtro));
                        break;

                        case "Livro":
                            query = bc.Emprestimos.Where(l => l.Livro.Titulo.Contains(filtro.Filtro));
                        break;

                        default:
                            query = bc.Emprestimos;
                        break;
                    }
                } else{
                    query = bc.Emprestimos;
                }
             return query.Include(e => e.Livro).ToList();

                
                
                
                //return bc.Emprestimos.Include(e => e.Livro).ToList();
            }
        }

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }
    }
}