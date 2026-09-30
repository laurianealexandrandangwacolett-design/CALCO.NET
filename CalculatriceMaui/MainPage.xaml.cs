namespace CalculatriceMaui;

public partial class MainPage : ContentPage
{
    private string saisie = "";
    private double premierNombre = 0;
    private string operateur = "";
    private bool nouvelleSaisie = true;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnChiffreClicked(object? sender, EventArgs e)
    {
        var bouton = (Button)sender!;

        if (nouvelleSaisie)
        {
            saisie = "";
            nouvelleSaisie = false;
        }

        if (bouton.Text == "." && saisie.Contains('.'))
            return;

        if (bouton.Text == "." && saisie == "")
            saisie = "0";

        saisie += bouton.Text;
        LblResultat.Text = saisie;
    }

    private void OnOperateurClicked(object? sender, EventArgs e)
    {
        var bouton = (Button)sender!;

        if (operateur != "" && !nouvelleSaisie)
            Calculer();
        else
            double.TryParse(LblResultat.Text, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out premierNombre);

        operateur = bouton.Text;
        LblOperation.Text = $"{Formater(premierNombre)} {operateur}";
        nouvelleSaisie = true;
    }

    private void OnEgalClicked(object? sender, EventArgs e)
    {
        if (operateur == "")
            return;

        double second = ParseResultat();
        LblOperation.Text = $"{Formater(premierNombre)} {operateur} {Formater(second)} =";
        Calculer();
        operateur = "";
        nouvelleSaisie = true;
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        saisie = "";
        premierNombre = 0;
        operateur = "";
        nouvelleSaisie = true;
        LblResultat.Text = "0";
        LblOperation.Text = "";
    }

    private void OnBackspaceClicked(object? sender, EventArgs e)
    {
        if (nouvelleSaisie || saisie.Length == 0)
            return;

        saisie = saisie.Substring(0, saisie.Length - 1);
        LblResultat.Text = saisie == "" || saisie == "-" ? "0" : saisie;
    }

    private void OnSigneClicked(object? sender, EventArgs e)
    {
        double valeur = ParseResultat();
        if (valeur == 0)
            return;

        valeur = -valeur;
        saisie = Formater(valeur);
        LblResultat.Text = saisie;
    }

    private void OnPourcentageClicked(object? sender, EventArgs e)
    {
        double valeur = ParseResultat() / 100;
        saisie = Formater(valeur);
        LblResultat.Text = saisie;
        nouvelleSaisie = true;
    }

    private void Calculer()
    {
        double second = ParseResultat();
        double resultat = premierNombre;

        switch (operateur)
        {
            case "+": resultat = premierNombre + second; break;
            case "-": resultat = premierNombre - second; break;
            case "×": resultat = premierNombre * second; break;
            case "÷":
                if (second == 0)
                {
                    LblResultat.Text = "Erreur";
                    premierNombre = 0;
                    operateur = "";
                    nouvelleSaisie = true;
                    return;
                }
                resultat = premierNombre / second;
                break;
        }

        premierNombre = resultat;
        saisie = Formater(resultat);
        LblResultat.Text = saisie;
    }

    private double ParseResultat()
    {
        double.TryParse(LblResultat.Text, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out double v);
        return v;
    }

    private static string Formater(double valeur)
    {
        return valeur.ToString("0.##########", System.Globalization.CultureInfo.InvariantCulture);
    }
}
