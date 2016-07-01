using System;
using System.Data;
using System.Text;
using Npgsql;

namespace PostgreSQL_Demo {
    class Program {
        static void Main(string[] args) {

            //POC de Select simples no banco PostgreSQL, utilizando o package  Npgsql
            //http://www.npgsql.org/about.html
          
            Insert();
            //Select();
        }

        public static void Insert()
        {
            //criando input
            Funcionario funcionarioEntrada = new Funcionario();
            funcionarioEntrada.Nome = "Funcionario de Teste";
            funcionarioEntrada.Idade = 25;
            funcionarioEntrada.Salario = 2000;

            //objeto de resposta
            Funcionario funcionarioDB = null;

            // String de conexão
            var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=TesteDB";

            // Variável para commando SQL
            var commandText = new StringBuilder();

            // Conexão do sql
            NpgsqlConnection dbConnection = null;

            // Comando SQL para pegar todas as informações da tabela PointsRule
            commandText.Append("Insert into public.funcionario(nome, idade, salario)");
            commandText.Append("values (@Nome, @Idade, @Salario)");

            try {
                // Usando string de conexão
                using (dbConnection = new NpgsqlConnection(connectionString)) {
                    // Criando o command com a string de conexão e string sql
                    using (var command = new NpgsqlCommand(commandText.ToString(), dbConnection)) {
                        // Definindo o tipo de comando
                        command.CommandType = CommandType.Text;
                        // Passando os parâmetros
                        command.Parameters.AddWithValue("@Nome", funcionarioEntrada.Nome);
                        command.Parameters.AddWithValue("@Idade", funcionarioEntrada.Idade);
                        command.Parameters.AddWithValue("@Salario", funcionarioEntrada.Salario);

                        //abrindo conexão
                        dbConnection.Open();

                        // Criando o dataReader
                        var retorno = command.ExecuteNonQuery();

                        // Preenche a classe PointsRule com as informações vindas do banco
                        if (retorno > 0) {
                           Console.WriteLine("Funcionario inserido no banco com sucesso!");
                        }

                    }

                    Console.Read();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);

            } finally {
                // Verifica se a conexão está aberta e se estiver, fecha
                if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                    dbConnection.Close();
            }

        }

        public static void Select()
        {

            //criando input
            Funcionario funcionarioEntrada = new Funcionario();
            funcionarioEntrada.Nome = "eduardo";

            //objeto de resposta
            Funcionario funcionarioDB = null;

            // String de conexão
            var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=TesteDB";

            // Variável para commando SQL
            var commandText = new StringBuilder();

            // Conexão do sql
            NpgsqlConnection dbConnection = null;

            // Comando SQL para pegar todas as informações da tabela PointsRule
            commandText.Append("SELECT nome, idade, salario ");
            commandText.Append("FROM public.Funcionario ");
            commandText.Append("WHERE nome = @Nome");

            try {
                // Usando string de conexão
                using (dbConnection = new NpgsqlConnection(connectionString)) {
                    // Criando o command com a string de conexão e string sql
                    using (var command = new NpgsqlCommand(commandText.ToString(), dbConnection)) {
                        // Definindo o tipo de comando
                        command.CommandType = CommandType.Text;
                        // Passando os parâmetros
                        command.Parameters.AddWithValue("@Nome", funcionarioEntrada.Nome);

                        //abrindo conexão
                        dbConnection.Open();

                        // Criando o dataReader
                        var dataReader = command.ExecuteReader();

                        // Preenche a classe PointsRule com as informações vindas do banco
                        if (dataReader.HasRows) {
                            while (dataReader.Read()) {
                                funcionarioDB = new Funcionario() {
                                    Nome = (dataReader["nome"]).ToString(),
                                    Idade = Convert.ToInt32(dataReader["idade"]),
                                    Salario = Convert.ToInt32(dataReader["salario"]),
                                };
                            }
                        }

                        if (funcionarioDB != null) {
                            Console.WriteLine("Funcionario Obtido");
                            Console.WriteLine("Nome:{0} | Idade: {1} | Salario: {2}", funcionarioDB.Nome, funcionarioDB.Idade, funcionarioDB.Salario);
                        }
                        Console.Read();
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);

            } finally {
                // Verifica se a conexão está aberta e se estiver, fecha
                if (dbConnection != null && dbConnection.State == ConnectionState.Open)
                    dbConnection.Close();
            }

        }

        public class Funcionario {
            public string Nome { get; set; }
            public int Idade { get; set; }
            public int Salario { get; set; }
        }
    }
}
