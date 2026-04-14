using System;
using System.IO;
// control
public class Aluno
{
	public string Nome { get; set; }
	public int MatriculaAluno { get; }
	public int Idade { get; set; }

	public Aluno(string nome, int matriculaAluno, int idade)
	{
		Nome = nome;
		MatriculaAluno = matriculaAluno;
		Idade = idade;
	}
}

public class Disciplina
{
	public string NomeDisciplina { get; set; }
	public int CodDisciplina { get; set; }
	public int NotaMinima { get; } = 7;

	public Disciplina(string nome, int codigo)
	{
		NomeDisciplina = nome;
		CodDisciplina = codigo;
	}
}

public class Matricula
{
	public int MatriculaAluno { get; set; }
	public int CodDisciplina { get; set; }
	public float Nota1 { get; set; }
	public float Nota2 { get; set; }

	public Matricula(int codDisciplina, float nota1, float nota2, int matriculaAluno)
	{
		CodDisciplina = codDisciplina;
		Nota1 = nota1;
		Nota2 = nota2;
		MatriculaAluno = matriculaAluno;
	}
}

// view
class Program
{
	static Aluno[] alunos = new Aluno[100];
	static int totalAlunos = 0;

	static Disciplina[] disciplinas = new Disciplina[100];
	static int totalDisciplinas = 0;

	static Matricula[] matriculas = new Matricula[100];
	static int totalMatriculas = 0;

	static int proximaMatricula = 1;
	static int proximoCodDisciplina = 1;

	static void Main()
	{
		CarregarDados();
		Menu();
		SalvarDados();
	}

	static void Menu()
	{
		int opcao;

		do
		{
			Console.WriteLine("\n===== SISTEMA ESCOLAR =====");
			Console.WriteLine("1- Cadastrar Aluno");
			Console.WriteLine("2- Cadastrar Disciplina");
			Console.WriteLine("3- Cadastrar Matrícula");
			Console.WriteLine("4- Listar Alunos (COM DISCIPLINAS)");
			Console.WriteLine("5- Pesquisar Aluno");
			Console.WriteLine("6- Salvar");
			Console.WriteLine("0- Sair");

			int.TryParse(Console.ReadLine(), out opcao);

			switch (opcao)
			{
				case 1: CadastrarAluno(); break;
				case 2: CadastrarDisciplina(); break;
				case 3: CadastrarMatricula(); break;
				case 4: ListarAlunos(); break;
				case 5: PesquisarAluno(); break;
				case 6: SalvarDados(); break;
			}

		} while (opcao != 0);
	}

		// control

	// ================= CADASTROS =================

	static void CadastrarAluno()
	{
		Console.Write("Nome: ");
		string nome = Console.ReadLine();

		Console.Write("Idade: ");
		int idade = int.Parse(Console.ReadLine());

		alunos[totalAlunos++] = new Aluno(nome, proximaMatricula++, idade);

		Console.WriteLine("Aluno cadastrado!");
	}

	static void CadastrarDisciplina()
	{
		Console.Write("Nome da disciplina: ");
		string nome = Console.ReadLine();

		disciplinas[totalDisciplinas++] = new Disciplina(nome, proximoCodDisciplina++);

		Console.WriteLine("Disciplina cadastrada!");
	}

	static void CadastrarMatricula()
	{
		Console.Write("Matrícula do aluno: ");
		int mat = int.Parse(Console.ReadLine());

		Console.Write("Código da disciplina: ");
		int cod = int.Parse(Console.ReadLine());

		Console.Write("Nota 1: ");
		float n1 = float.Parse(Console.ReadLine());

		Console.Write("Nota 2: ");
		float n2 = float.Parse(Console.ReadLine());

		matriculas[totalMatriculas++] = new Matricula(cod, n1, n2, mat);

		Console.WriteLine("Matrícula cadastrada!");
	}

	// ================= LISTAGEM COMPLETA =================

	static void ListarAlunos()
	{
		Console.WriteLine("\n=== LISTA COMPLETA DE ALUNOS ===");

		for (int i = 0; i < totalAlunos; i++)
		{
			Console.WriteLine("\n----------------------------------");
			Console.WriteLine($"Matrícula: {alunos[i].MatriculaAluno}");
			Console.WriteLine($"Nome: {alunos[i].Nome}");
			Console.WriteLine($"Idade: {alunos[i].Idade}");

			Console.WriteLine("Disciplinas:");

			bool temDisciplina = false;

			for (int j = 0; j < totalMatriculas; j++)
			{
				if (matriculas[j].MatriculaAluno == alunos[i].MatriculaAluno)
				{
					temDisciplina = true;

					string nomeDisciplina = BuscarNomeDisciplina(matriculas[j].CodDisciplina);

					float media = (matriculas[j].Nota1 + matriculas[j].Nota2) / 2;

					string situacao = media >= 7 ? "Aprovado" : "Reprovado";

					Console.WriteLine($"- {nomeDisciplina} | N1: {matriculas[j].Nota1} | N2: {matriculas[j].Nota2} | Média: {media:F2} | {situacao}");
				}
			}

			if (!temDisciplina)
				Console.WriteLine("Nenhuma disciplina cadastrada.");
		}
	}

	// ================= PESQUISA =================

	static void PesquisarAluno()
	{
		Console.Write("Digite nome ou matrícula: ");
		string entrada = Console.ReadLine();

		int matriculaBusca;
		bool ehNumero = int.TryParse(entrada, out matriculaBusca);

		for (int i = 0; i < totalAlunos; i++)
		{
			if ((ehNumero && alunos[i].MatriculaAluno == matriculaBusca) ||
				(!ehNumero && alunos[i].Nome.ToLower().Contains(entrada.ToLower())))
			{
				Console.WriteLine($"\nAluno: {alunos[i].Nome}");
				Console.WriteLine($"Matrícula: {alunos[i].MatriculaAluno}");

				MostrarDisciplinasDoAluno(alunos[i].MatriculaAluno);
			}
		}
	}

	static void MostrarDisciplinasDoAluno(int matriculaAluno)
	{
		Console.WriteLine("Disciplinas:");

		for (int i = 0; i < totalMatriculas; i++)
		{
			if (matriculas[i].MatriculaAluno == matriculaAluno)
			{
				string nomeDisciplina = BuscarNomeDisciplina(matriculas[i].CodDisciplina);

				float media = (matriculas[i].Nota1 + matriculas[i].Nota2) / 2;

				string situacao = media >= 7 ? "Aprovado" : "Reprovado";

				Console.WriteLine($"- {nomeDisciplina} | Média: {media:F2} | {situacao}");
			}
		}
	}

	static string BuscarNomeDisciplina(int codigo)
	{
		for (int i = 0; i < totalDisciplinas; i++)
		{
			if (disciplinas[i].CodDisciplina == codigo)
				return disciplinas[i].NomeDisciplina;
		}
		return "Desconhecida";
	}

	// ================= ARQUIVOS =================

	static void SalvarDados()
	{
		using (StreamWriter sw = new StreamWriter("alunos.dat"))
		{
			for (int i = 0; i < totalAlunos; i++)
				sw.WriteLine($"{alunos[i].MatriculaAluno};{alunos[i].Nome};{alunos[i].Idade}");
		}

		using (StreamWriter sw = new StreamWriter("disciplinas.dat"))
		{
			for (int i = 0; i < totalDisciplinas; i++)
				sw.WriteLine($"{disciplinas[i].CodDisciplina};{disciplinas[i].NomeDisciplina}");
		}

		using (StreamWriter sw = new StreamWriter("matriculas.dat"))
		{
			for (int i = 0; i < totalMatriculas; i++)
				sw.WriteLine($"{matriculas[i].MatriculaAluno};{matriculas[i].CodDisciplina};{matriculas[i].Nota1};{matriculas[i].Nota2}");
		}

		Console.WriteLine("Dados salvos!");
	}

	static void CarregarDados()
	{
		if (File.Exists("alunos.dat"))
		{
			var linhas = File.ReadAllLines("alunos.dat");
			foreach (var l in linhas)
			{
				var p = l.Split(';');
				int mat = int.Parse(p[0]);

				alunos[totalAlunos++] = new Aluno(p[1], mat, int.Parse(p[2]));

				if (mat >= proximaMatricula)
					proximaMatricula = mat + 1;
			}
		}

		if (File.Exists("disciplinas.dat"))
		{
			var linhas = File.ReadAllLines("disciplinas.dat");
			foreach (var l in linhas)
			{
				var p = l.Split(';');
				int cod = int.Parse(p[0]);

				disciplinas[totalDisciplinas++] = new Disciplina(p[1], cod);

				if (cod >= proximoCodDisciplina)
					proximoCodDisciplina = cod + 1;
			}
		}

		if (File.Exists("matriculas.dat"))
		{
			var linhas = File.ReadAllLines("matriculas.dat");
			foreach (var l in linhas)
			{
				var p = l.Split(';');

				matriculas[totalMatriculas++] =
					new Matricula(int.Parse(p[1]), float.Parse(p[2]), float.Parse(p[3]), int.Parse(p[0]));
			}
		}
	}
}
