using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class ProbabilityCalculator
    {
        public static double CalculateWinProbability(Dice dice1, Dice dice2)
        {
            int wins = 0;
            int total = dice1.Faces.Length * dice2.Faces.Length;

            foreach (var face1 in dice1.Faces)
            {
                foreach (var face2 in dice2.Faces)
                {
                    if (face1 > face2) wins++;
                }
            }

            return (double)wins / total;
        }

        public static string GenerateProbabilityTable(List<Dice> dice)
        {
            var rows = new List<string[]>();
            var header = new List<string> { "Dice" };
            header.AddRange(dice.Select(d => d.ToString()));
            rows.Add(header.ToArray());

            CollectRaw(rows, dice);

            int[] columnWidths = DeterminMaxWidth(rows);

            return CreateTable(rows, columnWidths);
        }

        public static int[] DeterminMaxWidth(List<string[]> rows)
        {
            int[] columnWidths = new int[rows[0].Length];
            for (int col = 0; col < columnWidths.Length; col++)
            {
                columnWidths[col] = rows.Max(row => row[col].Length);
            }
            return columnWidths;
        }

        public static void CollectRaw(List<string[]> rows, List<Dice> dice)
        {
            for (int i = 0; i < dice.Count; i++)
            {
                var row = new List<string> { dice[i].ToString() };
                for (int j = 0; j < dice.Count; j++)
                {
                    if (i == j)
                    {
                        row.Add("-");
                    }
                    else
                    {
                        var prob = CalculateWinProbability(dice[i], dice[j]);
                        row.Add($"{prob:P1}");
                    }
                }
                rows.Add(row.ToArray());
            }
        }

        public static string CreateTable(List<string[]> rows, int[] columnWidths)
        {
            var table = new StringBuilder();
            table.AppendLine("Probability table (row beats column):");
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    string cell = rows[i][j];
                    string formattedCell = cell.PadRight(columnWidths[j] + 2);
                    table.Append(formattedCell);
                }
                table.AppendLine();
            }

            return table.ToString();
        }
    }
}
