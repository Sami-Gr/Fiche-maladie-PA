using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tulpep.NotificationWindow;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;


namespace Pancréatite
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public string tdmtxt;
        public string echotxt;
        public string msg;
        public string comp;
        public string comp2;
        public string bmi;
        public double t;
        public double p;
        

        public Main()
        {
           InitializeComponent();
           createdir();

           Splash sp = new Splash();
           sp.ShowDialog();                       
        }

        public void createdir()
        {
            Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fiches Pancréatite"));
        }

        #region number-textbox
        private void age_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void poids_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void taille_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void imc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        #endregion

        #region echo
        public void echoabd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (echoabd.SelectedIndex == 0)
            {
                vesiculebiliaire.IsEnabled = true;
                paroivesiculaire.IsEnabled = true;
                VBP.IsEnabled = true;
                VBIH.IsEnabled = true;
                echotxt = ("• Echographie abdominale : "+echoabd.SelectedValue.ToString() + Environment.NewLine );
            }
            else
            {
                vesiculebiliaire.IsEnabled = false;
                paroivesiculaire.IsEnabled = false;
                VBP.IsEnabled = false;
                VBIH.IsEnabled = false;
                echotxt = ("• Echographie abdominale : " + echoabd.SelectedValue.ToString() + Environment.NewLine);
            }
        }

        private void vesiculebiliaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            echotxt += ("- Vésicule biliaire :" + vesiculebiliaire.SelectedValue.ToString() + Environment.NewLine);
        }

        private void paroivesiculaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            echotxt += ("- Paroi vésiculaire :" + paroivesiculaire.SelectedValue.ToString() + Environment.NewLine);
        }

        private void VBP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            echotxt += ("- VBP :" + VBP.SelectedValue.ToString() + Environment.NewLine);
        }

        private void VBIH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            echotxt += ("- VBIH :" + VBIH.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine);
        }
        #endregion

        #region TDM
        public void TDM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TDM.SelectedIndex == 0)
            {
                balthazard.IsEnabled = true;
                CTSI.IsEnabled = true;
                tdmtxt = ("TDM Abdominale: "+TDM.SelectedValue.ToString() + Environment.NewLine );
                
            }
            else
            {
                balthazard.IsEnabled = false;
                CTSI.IsEnabled = false;
                tdmtxt = ("TDM Abdominale: " + TDM.SelectedValue.ToString() + Environment.NewLine);
            }
            
        }

        public void balthazard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tdmtxt += ("- Classification de BALTHAZAR  :" + balthazard.SelectedValue.ToString() + Environment.NewLine);
        }

        public void CTSI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tdmtxt += ("- Computed Tomography  Severty Index (CTSI) = " + CTSI.SelectedValue.ToString() + Environment.NewLine);
        }
        #endregion

        private void PDF_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                createdir();
                updateimc();
                updatemcp();

                #region PDF
                string Filenm = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Fiches Pancréatite/Fiche pancréatite N°" + ndossier.Text +" -- "+ nom.Text + " " + prenom.Text +".pdf");
                FileStream fs = new FileStream(Filenm, FileMode.Create, FileAccess.Write, FileShare.None);
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                #endregion

                string fiche = ("FICHE DE SAISIE PANCREATITE AIGUE"+ Environment.NewLine+ Environment.NewLine + "Nom: " + nom.Text + Environment.NewLine + "Prénom: " + prenom.Text + Environment.NewLine + "N° DOSSIER: " + ndossier.Text + Environment.NewLine + "N° ANAPATH : " + nanapath.Text + Environment.NewLine + "N° de TEL : " + ntel.Text + Environment.NewLine + "Sexe: " + sexe.SelectedValue.ToString()+ Environment.NewLine +Environment.NewLine+"Date d’entrée: " + dateent.SelectedDate+ Environment.NewLine+ "Date d’intervention: " +dateint.SelectedDate+ Environment.NewLine + "Date de sortie: "+ datesor.SelectedDate+ Environment.NewLine+ Environment.NewLine+"Poids: " +poids.Text+"Kg"+ Environment.NewLine+ "Taille: " +taille.Text+"Cm"+ Environment.NewLine+ bmi +Environment.NewLine + Environment.NewLine+ "Diagnostic:"+dg.SelectedValue.ToString()+ Environment.NewLine + Environment.NewLine+ "ANTECEDENTS MEDICAUX: "+Environment.NewLine + Environment.NewLine+ "• Diabète : " +diabète.SelectedValue.ToString() + Environment.NewLine + "• HTA : " +hta.SelectedValue.ToString() + Environment.NewLine + "• Cardiopathie : "+cardiopathie.SelectedValue.ToString() + Environment.NewLine + "• Insuffisance respiratoire : "+insufresp.SelectedValue.ToString() + Environment.NewLine+ "• Insuffisance rénale : "+insufrenale.SelectedValue.ToString()+ Environment.NewLine + "• Insuffisance hépatique : " +insufhep.SelectedValue.ToString()+ Environment.NewLine + "• Adénome para-thyroidien : " +adéparathy.SelectedValue.ToString()+ Environment.NewLine + "• Maladie inflammatoire du tube digestif : " +mici.SelectedValue.ToString()+ Environment.NewLine + "• Colique hépatique : " +coliquehep.SelectedValue.ToString()+ Environment.NewLine + "• Lithiase vésiculaire : " +lithiasevésic.SelectedValue.ToString()+ Environment.NewLine + "• Lithiase de la voie biliaire principale : " +lithiasevbp.SelectedValue.ToString()+ Environment.NewLine + "• Lithiase intra-hépatique : " +lithiaseintra.SelectedValue.ToString()+ Environment.NewLine + "• Pancréatite aigue : " +pancaigue.SelectedValue.ToString()+ Environment.NewLine + "• Pancréatite chronique  : " +pancchro.SelectedValue.ToString()+ Environment.NewLine + "• Pancréatite aigue post-CPRE : " +pancpostcpre.SelectedValue.ToString()+ Environment.NewLine + Environment.NewLine + "* ATCD Chirurgicaux : " +atcdchir.SelectedValue.ToString()+ Environment.NewLine + Environment.NewLine + "* Habitudes toxiques : "+ Environment.NewLine + "• Alcoolisme : " +alcool.SelectedValue.ToString()+ Environment.NewLine + "• Tabagisme : " +tabac.SelectedValue.ToString()+ Environment.NewLine + "• Prise médicamenteuse pancréato-toxique : " +prisemed.SelectedValue.ToString()+ Environment.NewLine + Environment.NewLine + "* CLINIQUE"+ Environment.NewLine + Environment.NewLine + "Début de la symptomatologie :"+debutsymp.SelectedValue.ToString()+ Environment.NewLine+"Signes généraux : " + Environment.NewLine + "• Fièvre :"+fièvre.SelectedValue.ToString() + Environment.NewLine + "• Fréquence cardiaque :"+fc.SelectedValue.ToString() + Environment.NewLine + "• Fréquence respiratoire :"+fr.SelectedValue.ToString() + Environment.NewLine + "Signes fonctionnels :" + Environment.NewLine + "• Douleur :"+douleur.SelectedValue.ToString() + Environment.NewLine + "• Nausées  :"+nausée.SelectedValue.ToString() + Environment.NewLine + "• Vomissements :"+vomissement.SelectedValue.ToString() + Environment.NewLine + "• Arret des matières et des gaz :"+arretdesmat.SelectedValue.ToString() + Environment.NewLine + "• Distension abdominale :"+distensionfx.SelectedValue.ToString() + Environment.NewLine + "• Syndrome de choléstase clinique :"+sdcholestase.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "Examen abdominal a l’admission:" + Environment.NewLine + "• Inspection :" + Environment.NewLine + "- Position en chien de fusille :"+positionchien.SelectedValue.ToString() + Environment.NewLine + "- Distension abdominale: "+distensioninspection.SelectedValue.ToString() + Environment.NewLine + "- Cicatrice de laparotomie :"+cicatricelapa.SelectedValue.ToString() + Environment.NewLine + "- Signe de Cullen :"+signecullen.SelectedValue.ToString() + Environment.NewLine + "- Signe de Grey-Turner :"+signegrey.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "• Palpation :" + Environment.NewLine + "- Sensibilité localisée :"+sensibiliteloc.SelectedValue.ToString() + Environment.NewLine + "- Sensibilité généralisée :"+sensibilitegen.SelectedValue.ToString() + Environment.NewLine + "- Défense localisée :"+defenceloc.SelectedValue.ToString() + Environment.NewLine + "- Défense généralisée :"+defencegen.SelectedValue.ToString() + Environment.NewLine + "- Contracture :"+contracture.SelectedValue.ToString() + Environment.NewLine + "- Masse palpable :"+massepalpable.SelectedValue.ToString() + Environment.NewLine + "- Signe de Murphy :"+signemurphy.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "• Percussion" + Environment.NewLine + "- Matité des flancs :"+matiteeflanc.SelectedValue.ToString() + Environment.NewLine + "- Tympanisme :"+tympanisme.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "* Bilan étiologique à l’admission :" + Environment.NewLine + "• Bilan hépatique : "+bilanhepatique.SelectedValue.ToString() + Environment.NewLine + "• Calcémie : "+calcemie.SelectedValue.ToString() + Environment.NewLine + "• TG : "+TG.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine);
                string fiche2 = (Environment.NewLine+ "* Evaluation de la sévérité :" + Environment.NewLine + "- SIRS : " + Environment.NewLine + "- SIRS à l’admission : "+sirsadmission.SelectedValue.ToString() + Environment.NewLine + "- SIRS à 48H : "+sirs48.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine);
                string fiche3 = (Environment.NewLine+ "- CRP: " +CRP.SelectedValue.ToString() + Environment.NewLine + "- Hématocrite: "+hematocrite.SelectedValue.ToString() + Environment.NewLine + "- TLT: "+TLT.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "* BIOLOGIE" + Environment.NewLine + Environment.NewLine + "- GB : "+GB.SelectedValue.ToString() + Environment.NewLine + "- Urée : "+uree.Text + Environment.NewLine + "- Créatinémie :"+creatinemie.Text + Environment.NewLine + "- Ionogramme :" + Environment.NewLine + "- Kaliémie : "+kaliemie.SelectedValue.ToString() + Environment.NewLine + "- Natrémie : "+natremie.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine+ "* TRAITEMENT: " + Environment.NewLine+ Environment.NewLine + "- Allergie aux ATB : "+allergieatb.SelectedValue.ToString() + Environment.NewLine + "- Antibiothérapie : "+atb.SelectedValue.ToString() + Environment.NewLine + "- Type d’ATB : "+typeatb.SelectedValue.ToString() + Environment.NewLine + "• ANTALGIQUES : " + Environment.NewLine + "- Paracétamol : "+paracetamol.SelectedValue.ToString() + Environment.NewLine + "- Acupan : "+acupan.SelectedValue.ToString() + Environment.NewLine + "- Temgésic : "+tamgesic.SelectedValue.ToString() + Environment.NewLine + "- Disparition de la douleur : "+disparitiondlr.SelectedValue.ToString() + Environment.NewLine + "- Reprise de l'alimentation orale : "+repriseorale.SelectedValue.ToString() + Environment.NewLine + "- Reprise de la douleur : "+reprisedlr.SelectedValue.ToString() + Environment.NewLine+ Environment.NewLine + "* TRAITEMENT CHIRURGICAL : " + Environment.NewLine + "• Dans la même hospitalisation : "+chirmemehosp.SelectedValue.ToString() + Environment.NewLine + "- Intervention : "+intervention.SelectedValue.ToString() + Environment.NewLine + "- durée de l’intervention : "+dureeintervention.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "- Exploration : " + Environment.NewLine + "• Pédiculite : "+pediculite.SelectedValue.ToString() + Environment.NewLine + "• Adhérence : "+adherence.SelectedValue.ToString() + Environment.NewLine + "• Fistule cholecysto-duodénale : "+fistule.SelectedValue.ToString() + Environment.NewLine + "• VB : "+expvb.SelectedValue.ToString() + Environment.NewLine + "• VBP : "+expvbp.SelectedValue.ToString() + Environment.NewLine + "• Canal cystique : "+expcanalcys.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "• Cholecystectomie : "+cholecystectomie.SelectedValue.ToString() + Environment.NewLine + "• Geste effectué : "+gesteeffectue.SelectedValue.ToString() + Environment.NewLine + "• Individualisation des éléments biliaires et vasculaire : "+indivudialisation.SelectedValue.ToString() + Environment.NewLine + "Geste associé: " + Environment.NewLine + "- Complications per opératoire : "+compperop.SelectedValue.ToString() + Environment.NewLine + "- Geste en rapport avec le traumatisme VBP : "+gestetrmvbp.SelectedValue.ToString() + Environment.NewLine + "- Cholangiographie per opératoire : "+cholangioperop.SelectedValue.ToString() + Environment.NewLine + "- Drainage per opératoire : "+drainageperop.SelectedValue.ToString() + Environment.NewLine + "- Conversion : "+conversion.SelectedValue.ToString() + Environment.NewLine + "- Motif de conversion : "+motifconversion.SelectedValue.ToString() + Environment.NewLine + "- Durée entre le diagnostic et le traitement chirurgical : "+dureedgtrt.SelectedValue.ToString() + Environment.NewLine + "- Suite opératoires : "+suitesop.SelectedValue.ToString() + Environment.NewLine + "- Complications post opératoire précoces : "+comp + Environment.NewLine + "- Complications post opératoire tardives : "+comp2 + Environment.NewLine + "- Cause de décès : "+causedeces.SelectedValue.ToString() + Environment.NewLine + "- Séjours hospitaliers :"+sejourshosp.SelectedValue.ToString() + Environment.NewLine + "- Séjours post opératoire : "+sejourspostop.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "- TRT de sortie : " + Environment.NewLine + "- Antibiotique : "+atbsortie.SelectedValue.ToString() + Environment.NewLine + "- Antalgique : "+antalgiquesortie.SelectedValue.ToString() + Environment.NewLine + "- Anticoagulants : "+anticoagulants.SelectedValue.ToString() + Environment.NewLine + "Examen Ana-pathologique : "+exanapath.SelectedValue.ToString() + Environment.NewLine + "Follow up : " + Environment.NewLine + Environment.NewLine + "1mois : " + Environment.NewLine + "persistence de la douleur : "+_1moispers.SelectedValue.ToString() + Environment.NewLine + "Eventration : "+_1moiev.SelectedValue.ToString() + Environment.NewLine + "Ictère : "+_1moiictere.SelectedValue.ToString() + Environment.NewLine + Environment.NewLine + "3mois : " + Environment.NewLine + "Persistence de la douleur : "+_3moipers.SelectedValue.ToString() + Environment.NewLine + "Eventration : "+_3moiev.SelectedValue.ToString() + Environment.NewLine + "Ictère : "+_3moiictere.SelectedValue.ToString() + Environment.NewLine);
                doc.Add(new iTextSharp.text.Paragraph(fiche+echotxt+fiche2+tdmtxt+fiche3));
                doc.Close();

                #region notification
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Succès..";
                popup.ContentText = "Votre fichier est enregistré dans: Mes Documents/Fiches Pancréatite.";
                popup.Popup();
                #endregion
            }

            catch (System.Exception)
            {                               
                error();
            }

        }

        public void updatemcp()
        {
            try
            {
                comp = "";
                comp2 = "";
                foreach (KeyValuePair<string, object> s in mcp.SelectedItems)
                {
                    string msg = s.Key.ToString();
                    comp += msg + " / ";
                }

                foreach (KeyValuePair<string, object> s in mcp2.SelectedItems)
                {
                    string msg2 = s.Key.ToString();
                    comp2 += msg2 + " / ";
                }
            }

            catch (System.NullReferenceException)
            {
                error();
            }
            //MessageBox.Show(comp);
            // MessageBox.Show(comp2);
        }              

        public void updateimc()
        {
            try
            {
                t = (double.Parse(taille.Text)) / 100;
                p = double.Parse(poids.Text);
                double tt = t * t;
                double c = (p / tt);
                bmi = ("IMC = " + c.ToString());
                IMC.Content = bmi;
            }
            catch(FormatException)
            {
                #region notification
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Erreur..";
                popup.ContentText = "Veuillez compléter les champs:Taille et poids";
                popup.Popup();
                #endregion
            }
        }

        public void imcbtn_Click(object sender, RoutedEventArgs e)
        {
            updateimc();
        }

        private void parcourir_Click(object sender, RoutedEventArgs e)
        {
            createdir();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += "/Fiches Pancréatite";
            Process.Start(path);
        }   

        public void error()
        {
            
            #region notification             
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Erreur...";
                popup.ContentText = "Prière de remplir toutes les cases vides.";
                popup.Popup();
                #endregion           
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
       
}
