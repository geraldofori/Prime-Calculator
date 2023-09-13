using System;
using Gtk;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected async void Calculate(object sender, EventArgs e)
    {
        string inputText1 = entry1.Text;
        string inputText2 = entry2.Text;

        var tasks = new List<Task<List<int>>>();

        if (!string.IsNullOrWhiteSpace(inputText1) && int.TryParse(inputText1, out int inputValue1))
        {
            tasks.Add(CalculatePrimesAsync(inputValue1, textview1.Buffer));
        }
        else
        {
            textview1.Buffer.Clear();
        }

        if (!string.IsNullOrWhiteSpace(inputText2) && int.TryParse(inputText2, out int inputValue2))
        {
            tasks.Add(CalculatePrimesAsync(inputValue2, textview2.Buffer));
        }
        else
        {
            textview2.Buffer.Clear();
        }

        await Task.WhenAll(tasks);
    }

    private async Task<List<int>> CalculatePrimesAsync(int maxNumber, TextBuffer textBuffer)
    {
        var primes = await Task.Run(() => CalculatePrimes(maxNumber));
        textBuffer.Text = string.Join("\n", primes);
        return primes;
    }

    private List<int> CalculatePrimes(int maxNumber)
    {
        List<int> primes = new List<int>();

        for (int number = 2; number <= maxNumber; number++)
        {
            bool isPrime = true;

            for (int divisor = 2; divisor <= Math.Sqrt(number); divisor++)
            {
                if (number % divisor == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            if (isPrime)
            {
                primes.Add(number);
            }
        }

        return primes;
    }

    protected void Reset(object sender, EventArgs e)
    {
        entry1.Text = entry2.Text = "";
        textview1.Buffer.Clear();
        textview2.Buffer.Clear();
    }
}
