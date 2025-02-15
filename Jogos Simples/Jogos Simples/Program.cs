

class Menu 
{
    public static void Main()
    {
        Console.WriteLine("Bem Vindo, qual Jogo pretende Jogar?");
        Console.WriteLine("1 - Jogo do Galo");
        Console.WriteLine("2 - Sudoku");


        while (true)
        {
            int opcao = 0;
            Console.WriteLine("Escolha uma opção:");
            opcao = Convert.ToInt32(Console.ReadLine());
            switch (opcao)
            {
                case 1:
                    JogoDoGalo jogo = new JogoDoGalo();
                    jogo.Iniciar();
                    break;
                case 2:
                    Sudoku jogo2 = new Sudoku();
                    jogo2.Iniciar();
                    break;
                default:
                    Console.WriteLine("Opção Inválida");
                    break;
            }
            
        }
       
    }
}

class JogoDoGalo
{
    static char[] tabuleiro = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
    public void Iniciar()
    {
        Console.WriteLine("Deseja Começar? [Y/N]");
        string resposta = Console.ReadLine();

        if (resposta == "Y" || resposta == "y")
        {
            Console.WriteLine("Vamos Começar");
            Jogar();
        }
        else
        {
            Menu.Main();
        }
    }

    private void Jogar()
    {
        bool fimjogo = false;
        int numeromaxrodadas = 9;
        bool quemcomeça = false;
        Random rand = new Random();
       
        
        if (rand.Next(0, 2) != 0)
        {
            quemcomeça = true;
        }

        ExibirTabuleiro();
        while (!fimjogo && numeromaxrodadas > 0)
        {
            if (quemcomeça)
            {
                ComecoPlayer();
            }
            else
            {
                ComecoBot();
            }

            numeromaxrodadas--;
            quemcomeça = !quemcomeça; // Alterna entre player e bot

            ExibirTabuleiro(); // Mostra o tabuleiro atualizado

            if (VerificarVencedor())
            {
                fimjogo = true;
                Console.WriteLine("Fim de Jogo!");
                
                Console.WriteLine("Deseja Jogar Novamente? [Y/N]");
                string resposta = Console.ReadLine();
                if (resposta == "Y" || resposta == "y")
                {
                    Console.Clear();
                    tabuleiro = new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
                    Iniciar();
                }
                else
                {
                    Console.Clear();
                    Menu.Main();
                }
            }
        }



    }

    static void ExibirTabuleiro()
    {
        Console.WriteLine(@$"
     |     |     
  {tabuleiro[0]}  |  {tabuleiro[1]}  |  {tabuleiro[2]}  
_____|_____|_____
     |     |     
  {tabuleiro[3]}  |  {tabuleiro[4]}  |  {tabuleiro[5]}  
_____|_____|_____
     |     |     
  {tabuleiro[6]}  |  {tabuleiro[7]}  |  {tabuleiro[8]}  
     |     |     
");
    }

    static void ComecoPlayer()
    {
        Console.WriteLine("Escolha a sua posição");
        string posicao = Console.ReadLine();
        Console.WriteLine("Escolheu a posição: " + posicao);
        posicaomanager( posicao, 'X');
        
        
    }

    static void ComecoBot()
    {
        Console.WriteLine("Bot está escolhendo uma posição...");
        int posicao = MelhorJogada();
        if (posicao != -1)
        {
            Console.WriteLine($"Bot escolheu a posição: {posicao + 1}");
            posicaomanager((posicao + 1).ToString(), 'O');
        }
        

    }

    static int MelhorJogada()
    {
        // 1. Tenta ganhar
        for (int i = 0; i < 9; i++)
        {
            if (tabuleiro[i] == ' ')
            {
                tabuleiro[i] = 'O';
                if (VerificarVencedor()) return i;
                tabuleiro[i] = ' '; // Desfaz a jogada
            }
        }

        // 2. Bloqueia o jogador se ele puder ganhar
        for (int i = 0; i < 9; i++)
        {
            if (tabuleiro[i] == ' ')
            {
                tabuleiro[i] = 'X';
                if (VerificarVencedor())
                {
                    tabuleiro[i] = ' '; // Desfaz a jogada
                    return i;
                }
                tabuleiro[i] = ' '; // Desfaz a jogada
            }
        }

        // 3. Escolhe o centro se estiver livre
        if (tabuleiro[4] == ' ') return 4;

        // 4. Escolhe um dos cantos disponíveis
        int[] cantos = { 0, 2, 6, 8 };
        foreach (int canto in cantos)
        {
            if (tabuleiro[canto] == ' ') return canto;
        }

        // 5. Escolhe qualquer posição livre
        for (int i = 0; i < 9; i++)
        {
            if (tabuleiro[i] == ' ') return i;
        }

        return -1; // Algo deu errado (não deveria acontecer)
    }

    static bool VerificarVencedor()
    {
        int[][] combinacoesVitoria = {
        new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8}, // Linhas
        new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8}, // Colunas
        new int[] {0, 4, 8}, new int[] {2, 4, 6}                        // Diagonais
    };

        foreach (int[] combo in combinacoesVitoria)
        {
            if (tabuleiro[combo[0]] != ' ' &&
                tabuleiro[combo[0]] == tabuleiro[combo[1]] &&
                tabuleiro[combo[1]] == tabuleiro[combo[2]])
            {
                Console.WriteLine($"O vencedor é {tabuleiro[combo[0]]}!");
                return true;
            }
            else if (!tabuleiro.Contains(' '))
            {
                Console.WriteLine("Empate!");
                return true;
            }
        }
        return false;
    }

    static void posicaomanager(string posicao, char simbolo)
    {
        if (int.TryParse(posicao, out int pos) && pos >= 1 && pos <= 9)
        {
            pos--; // Converte para índice do array
            if (tabuleiro[pos] == ' ')
            {
                tabuleiro[pos] = simbolo;
            }
            else
            {
                Console.WriteLine("Posição ocupada! Escolha outra.");
                if (simbolo == 'X') ComecoPlayer(); // Se for player, pede nova jogada
            }
        }
        else
        {
            Console.WriteLine("Posição inválida!");
            if (simbolo == 'X') ComecoPlayer(); // Se for player, pede nova jogada
        }
    }

}

class Sudoku
{
    public void Iniciar()
    {
        Console.WriteLine("Deseja Começar? [Y/N]");
        string resposta = Console.ReadLine();

        if (resposta == "Y" || resposta == "y")
        {
            Console.WriteLine("Vamos Começar");
            Jogar();
        }
        else
        {
            Menu.Main();
        }
    }

    static void Jogar()
    {
        Console.WriteLine("Sudoku");
    }
}