using System;
using System.Collections.Generic;

class Program
{
    static bool[,] grid;
    static Stack<(int, char)> moveHistory = new Stack<(int, char)>();
    static int width;
    static int height;
    static char currentPlayer = 'H';

    static void Main()
    {
        // Etape 1 - Initialisation
        width = ConsoleReadDimension("Largeur", 2, 20);
        height = ConsoleReadDimension("Hauteur", 2, 20);
        grid = new bool[height, width];
        bool isGameOver = false;

        // Boucle de jeu
        while (!isGameOver)
        {
            PrintGrid();
            Console.WriteLine($"Joueur {currentPlayer}, entrez un numéro de case, Z pour annuler le dernier coup, Y pour refaire un coup annulé, ou Q pour quitter:");
            var saisie = Console.ReadLine();

            if (int.TryParse(saisie, out var numero) && IsValidMove(numero, currentPlayer))
            {
                MakeMove(numero, currentPlayer);
                currentPlayer = currentPlayer == 'H' ? 'V' : 'H';
            }
            else if (saisie.ToUpper() == "Q")
            {
                Console.WriteLine("Jeu terminé.");
                break;
            }
            else if (saisie.ToUpper() == "Z")
            {
                UndoMove();
                currentPlayer = currentPlayer == 'H' ? 'V' : 'H';
            }
            else if (saisie.ToUpper() == "Y")
            {
                RedoMove();
            }
            else
            {
                Console.WriteLine("Saisie invalide. Veuillez réessayer.");
            }

            isGameOver = CheckGameOver();
        }

        if (isGameOver)
        {
            Console.WriteLine("Plus aucun coup n'est possible. Fin de la partie !");
        }
    }

    static int ConsoleReadDimension(string nom, int min, int max)
    {
        int val;
        Console.Write($"{nom} du plateau ({min}-{max}) : ");
        while (!int.TryParse(Console.ReadLine(), out val) || val < min || val > max)
        {
            Console.Error.WriteLine($"Valeur incorrecte. Veuillez entrer un entier compris entre {min} et {max}.");
            Console.Write($"{nom} du plateau ({min}-{max}) : ");
        }
        return val;
    }


    static void PrintGrid()
    {
        Console.WriteLine("  +" + string.Join("+", new string('-', 3).PadRight(width * 4 - 1, '+')) + "+");

        for (int i = 0; i < height; i++)
        {
            // Affiche les numéros des cases avec les bordures verticales
            Console.Write("  |");
            for (int j = 0; j < width; j++)
            {
                if (grid[i, j])
                {
                    Console.Write("XX |");
                }
                else
                {
                    Console.Write($"{(i * width + j + 1),2} |");
                }
            }
            Console.WriteLine();

            // Affiche les bordures horizontales des cases
            Console.WriteLine("  +" + string.Join("+", new string('-', 3).PadRight(width * 4 - 1, '+')) + "+");
        }
    }

    static bool IsValidMove(int moveNumber, char player)
    {
        int x = (moveNumber - 1) % width;
        int y = (moveNumber - 1) / width;

        if (player == 'H')
        {
            // Pour le joueur H, vérifiez la case actuelle et celle du dessus
            return y > 0 && !grid[y, x] && !grid[y - 1, x];
        }
        else
        {
            // Pour le joueur V, vérifiez la case actuelle et celle de droite (comme avant)
            return x < width - 1 && !grid[y, x] && !grid[y, x + 1];
        }
    }

    static void MakeMove(int moveNumber, char player)
    {
        int x = (moveNumber - 1) % width;
        int y = (moveNumber - 1) / width;

        if (player == 'H')
        {
            grid[y, x] = grid[y - 1, x] = true; // Marquez la case actuelle et celle du dessus pour le joueur H
        }
        else
        {
            grid[y, x] = grid[y, x + 1] = true; // Marquez la case actuelle et celle de droite pour le joueur V
        }

        moveHistory.Push((moveNumber, player)); // Empilez le numéro de la case et le joueur
    }

    static void UndoMove()
    {
        if (moveHistory.Count > 0)
        {
            var (lastMove, player) = moveHistory.Pop();
            int x = (lastMove - 1) % width;
            int y = (lastMove - 1) / width;

            if (player == 'H' && y > 0)
            {
                grid[y, x] = grid[y - 1, x] = false; // Annulez le coup pour le joueur H
            }
            else if (player == 'V' && x < width - 1)
            {
                grid[y, x] = grid[y, x + 1] = false; // Annulez le coup pour le joueur V
            }
        }
        else
        {
            Console.WriteLine("Aucun coup à annuler.");
        }
    }


    static void RedoMove()
    {
        // La fonctionnalité de refaire n'est pas implémentée ici pour simplifier.
        Console.WriteLine("Fonctionnalité 'Refaire' non implémentée.");
    }

    static bool CheckGameOver()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                if (!grid[i, j] && !grid[i, j + 1])
                {
                    return false;
                }
            }
        }
        return true;
    }
}

