using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluedo_TarnusGabriel
{
    struct Carte //Variable custom pour mes cartes
    { 
        public string carteName;    //Contenu de la carte, qu'est-ce qu'elle représente
        public bool distribue;      //Booléen permettant d'éviter de distribuer deux fois la carte (true : distribué, false : distribuable)
        public int joueur;          //Entier contenant le numéro du joueur qui détient la carte une fois distribué

        public Carte(string carteName, bool distribue, int joueur) //Constructeur de la structure "Carte", permet d'initialiser un carte avec des valeurs 
        {
            this.carteName = carteName; //Met la valeur de carteName dans la structure
            this.distribue = distribue; //Met la valeur de distribue dans la structure
            this.joueur = joueur;       //Met la valeur de joueur dans la structure

        }

    }

    class Program
    {
        static void Main(string[] args) //Fonction principale pour appeler les autres fonctions et intéragir avec le joueur
        {           
            Console.WriteLine("Bienvenue au Cluedo 5TTI!, le jeu se fait à trois joueur chacun pour soi. Voulez-vous jouer?");  //Proposition aux joueurs de jouer au Cluedo
            string jouer = Console.ReadLine(); //Variable pour demander si les utilisateurs désirent jouer + extraction de ce que les joueurs ont écrits

            if (jouer == "oui")
            {
                string rejouer = ""; //Variable pour demander à l'utilisateur si il voudra rejouer ou non

                do
                {
                    Carte[] jeu = new Carte[21]    //Création tu tableau des cartes
                    {                              //Insertion des données

                        new Carte("Vandjick", false, 0),
                        new Carte("Cour", false, 0),
                        new Carte("Internat", false, 0),
                        new Carte("Local 421", false, 0),
                        new Carte("Vestiaires", false, 0),
                        new Carte("Etude", false, 0),
                        new Carte("Parking", false, 0),
                        new Carte("Accueil", false, 0),
                        new Carte("Local 328", false, 0),
                        new Carte("Batte de baseball", false, 0),
                        new Carte("Bic", false, 0),
                        new Carte("Raquette", false, 0),
                        new Carte("Morphine", false, 0),
                        new Carte("Chaise", false, 0),
                        new Carte("Livre d'orthographe", false, 0),
                        new Carte("Romane Grootaers", false, 0),
                        new Carte("Ewan Ramsamy", false, 0),
                        new Carte("Dorian Michaux", false, 0),
                        new Carte("Ricardo Galvao", false, 0),
                        new Carte("Arthur Ryckbosch", false, 0),
                        new Carte("Victor Pholien", false, 0)

                    };

                    
                    string choix;   //Variable pour proposer au joueur de faire des hypothèses ou la dénonciation

                    bool fini = false;              //Bool qui va permettre de mettre fin au jeu une fois qu'il est gagné ou perdu
                    bool victoire = false;  //Bool pour mettre fin au jeu si le jeu est fini par un gagnant ($fini = true)
                    bool perdu = false; //Bool qui mets fin au jeu si le jeu est fini par l'élémination de tous les joueurs ($perdu = true)
                    int comptePerd = 0; //Variable qui va compter le nombre de perdant. Si $comptePerd = 3, la partie est fini ($perdu = true)

                    string[] etui = new string[3];  //Variable de l'étui (Contiendra la solution du jeu. 3 lignes : 3 cartes (1 de chaque type))f

                    int[,] cardJ = new int[3, 6];           //Tableau à double dimensions qui va contenir les indices de chaque cartes donnés aux joueurs (3 lignes = 3 joueurs, 6 colonnes = 6 cartes)
                    string[,] affCartes = new string[3, 6];     //Tableau à une dimension qui affichera le contenu des cartes à chacun des joueurs quand ces derniers le souhaite (3 lignes = 3 joueurs, 6 colonnes = 6 cartes)

                    string[,,] fiches = new string[3, 21, 2];    //Tableau à triples dimensions qui va contenir chacune des fiches de joueurs (3 tableaux 2D : 3 fiches -> 3 joueurs, 21 lignes : 21 cartes, 2 colonnes : colonne carte + colonne élémination (pour faire sa théorie))
                    string[] afFiche = new string[3];           //Tableau à une dimension pour afficher chaque fiche à son joueur respectif (3 lignes : 3 joueurs)

                    string[] hypothese = new string[3];         //Variable où les joueurs notent les trois cartes qu'ils suspectent
                    string[] denonciation = new string[3];      //Variable où les joueurs noteront leur hypothèse final 
                    string[] carteTrouve = new string[3];       //Variable qui va stocker les cartes révélés par rapport à $hypothese pour aider à la mise à jour de la fiche de jeu du joueur qui a rentré l'hypothèse

                    bool[] elimine = new bool[3] { false, false, false };   //Variable pour savoir si les joueurs sont éliminés ou pas pour si oui les empêcher de jouer
                    bool[] gagne = new bool[3] { false, false, false };     //Variable pour trouver le gagnant parmis les trois joueurs pour ensuite mettre fin au jeu

                    string[] victimes = new string[6] { "Abdulrahman Naji", "Lukas Debry", "Thomas van Goethem", "Nathan Leemans", "Damien Pelaez-Diaz", "Simon Abras" };   //Variable pour choisir la personne tué
                    string[] decouverte = new string[4] { "dans le distributeur, on se demande si la canette sera fraiche", "sur le pique à drapeau, il va glisser pendant longtemps", "dans la poubelle, le malheureux pue les déchets plus que la mort", "dans une chambre d'interne, qu'est-ce qu'il faisait là?" }; //Variable pour choisir le lieu où le cadavre a été découvert
                    string[] decouvreur = new string[6] { "Monsieur Kabisa", "Monsieur de Mahieu", "Madame Baudson", "Madame Kesch", "Monsieur Luque", "Madame Gustin" };   //Variable qui désigne la personne trouvant le cadavre

                    int nJoueur = 0;    //Compteur qui permettra à chaque joueur de joueuer et de modifier les données personnelles du joueur en train de jouer

                    Random vict = new Random();     //Random pour choisir au hasard la victime
                    Random decou = new Random();    //Random pour choisir au hasard le lieu de découverte
                    Random decour = new Random();   //Random pour choisir au hasard la personne découvrant le cadavre

                    int rnd1 = vict.Next(0, 6);     //Variable pour récupérer la valeur de $vict
                    int rnd2 = decou.Next(0, 4);    //Variable pour récupérer la valeur de $decou
                    int rnd3 = decour.Next(0, 6);   //Variable pour récupérer la valeur de $decour

                    Console.Clear();    //Commande pour rafraîchir l'écran. Principalement utilise quand les joueurs veulent refaire une partie

                    CreationEtui(ref etui, ref jeu);    //Appel de la fonction qui va créer l'étui   

                    Distribution(ref jeu);  //Distribution des cartes aux joueurs

                    PlacementCartes(jeu, ref cardJ);                    //Fonction qui va placer les indices des cartes à leurs joueurs réspectifs, permettra d'afficher les cartes à leurs joueurs
                    AffichagePlacement(ref affCartes, cardJ, jeu);      //Placement des valeurs des cartes dans le tableau d'affichage des cartes

                    FichesDeJeu(ref fiches, jeu);           //Fonction qui va créer les fiches de jeu et les ranger dans $fiches

                    ActuFicheDebut(ref fiches, affCartes);  //Fonction ppour déjà éléminer de la liste de suspicion du joueur x les cartes que ce dernier détient
                    AfficheFiches(ref afFiche, fiches);     //Fonction qui prépare les futurs affichages des fiches
                   
                    Console.WriteLine("CATASTROPHE, " + decouvreur[rnd3] + " en se baladant dans l'école a découvert le cadavre de son élève préféré, " + victimes[rnd1] + " " + decouverte[rnd2]);  //Création de la phrase de départ
                    Console.ReadLine(); //Permet au joueur de lire la phrase avant de continuer le jeu
                    do
                    {
                        for (nJoueur = 0; nJoueur < afFiche.Length; nJoueur++)  //Compteur pour faire participer chaque joueur
                        {
                            if (victoire == false || perdu == false)    //Empêcher la continuité du jeu si tout le monde a perdu ou si il y a un gagnant
                            {
                                if (elimine[nJoueur] == false)  //Empêche un joueur de jouer si il a été éléminé
                                {
                                    Console.Clear();    //Nettoyage de l'interface
                                    Console.WriteLine("FICHE DE JEU:");     //Présente la fiche de jeu ci-dessous
                                    Console.WriteLine(afFiche[nJoueur]);    //Affichage de la fiche de jeu du joueur $nJoueur

                                    Console.WriteLine("CARTES :");  //Présente les cartes ci-dessous

                                    for (int affiCarte = 0; affiCarte < affCartes.GetLength(1); affiCarte++)    //Compteur pour parcourir chaque cartes du joueur $nJoueur
                                    {
                                        Console.WriteLine(" " + affCartes[nJoueur, affiCarte]);   //Affichage des cartes du joueur $nJoueur
                                    }

                                    Console.WriteLine("");

                                    Console.WriteLine("Joueur " + (nJoueur + 1) + ", voulez-vous émettre une hypothèse?");  //Demande au joueur $nJoueur si il aimerait faire une hypothèse
                                    choix = Console.ReadLine(); //Extraction du texte écrit

                                    if (choix == "oui") //Si le joueur veut faire une hypothèse, alors...
                                    {
                                        Hypotheses(jeu, ref hypothese, nJoueur, ref carteTrouve, fiches);   //Appel de la fonction des hypothèses
                                        AfficheFiches(ref afFiche, fiches); //Actualisation de la fiche de joueur au niveau affichage
                                    }
                                }
                            }

                        }

                        for (nJoueur = 0; nJoueur < afFiche.Length; nJoueur++)  //Compteur pour faire participer chaque joueur
                        {
                            if (victoire == false || perdu == false)    //Empêcher la continuité du jeu si tout le monde a perdu ou si il y a un gagnant
                            {
                                if (elimine[nJoueur] == false)  //Empêche un joueur de jouer si il a été éléminé
                                {
                                    Console.Clear();                        //Nettoyage de l'interface
                                    Console.WriteLine("FICHE DE JEU:");     //Présente la fiche de jeu ci-dessous
                                    Console.WriteLine(afFiche[nJoueur]);    //Affichage de la fiche de jeu du joueur $nJoueur

                                    Console.WriteLine("CARTES :");  //Présente les cartes ci-dessous

                                    for (int affiCarte = 0; affiCarte < affCartes.GetLength(1); affiCarte++)    //Compteur pour parcourir chaque cartes du joueur $nJoueur
                                    {
                                        Console.WriteLine(" " + affCartes[nJoueur, affiCarte]);   //Affichage des cartes du joueur $nJoueur
                                    }

                                    Console.WriteLine(" ");

                                    Console.WriteLine("Joueur " + (nJoueur + 1) + ", voulez-vous faire votre dénonciation?");   //Demande au joueur $nJoueur si il désirerait faire sa dénonciation
                                    choix = Console.ReadLine(); //Extraction du text écrit

                                    if (choix == "oui") //Si le joueur désire faire sa dénonciation maintenant, alots...
                                    {
                                        Denonciation(nJoueur, ref denonciation, elimine, etui, gagne, ref comptePerd);  //Appel de la fonction de la dénonciation
                                    }
                                }

                            }
                        }


                        for(int detecGagne = 0; detecGagne < gagne.Length; detecGagne++)    //Compteur pour parcourir $gagne
                        {                            
                            if (gagne[detecGagne] == true)  //Si il y a 1 gagnant, alors...
                            {
                                victoire = true;    //Le jeu est terminé par une victoire d'un joueur
                                fini = true;        //Le jeu est terminé
                            }
                        }

                        if(comptePerd == 3) //Si tous les joueurs ont perdu, alors...
                        {
                            perdu = true;   //Le jeu est terminé par une défaite des trois joueurs
                            fini = true;    //Le jeu est terminé
                        }

                    } while (fini == false);    //Refaire le procéder du jeu si le jeu n'est pas fini

                    Console.Clear();    //Nettoyage de l'interface

                    if (perdu == true)  //Si le jeu a été perdu, alors...
                    {
                        Console.WriteLine("Hélas, tous les enquêteurs ont échoués, on ne pourra jamais porter justice au malheureux " + victimes[rnd1] + "... Quelle indignité...");    //Message de culpabilisation envers les joueurs parce qu'ils ont perdu
                        Console.ReadLine(); //ReadLine pour qu'ils puissent lire le texte à leur aise

                        Console.WriteLine("La solution était : " + etui[0] + ", " + etui[1] + " et " + etui[2]);    //Affichage de la solution que les joueurs auraient du trouver
                        Console.ReadLine(); //ReadLine pour qu'ils puissent lire le texte à leur aise
                    }
                    else   //Sinon...
                    {
                        if (victoire == true)   //Si le jeu a été mis fin par une victoire, alors...
                        {
                            Console.WriteLine("Encore bravo au gagnant! " + etui[2] + " paiera pour avoir commis ce crime. Contre le gré de Madame Lalaoui, elle subira la peine de mort!!!");  //Message de félécitation au gagnant et châtiment du tueur
                            Console.ReadLine(); //ReadLine pour qu'ils puissent lire le texte à leur aise
                        }
                    }

                    Console.WriteLine("La partie est fini! Désirez-vous en refaire une?");  //Demande aux joueurs si ils voudraient refaire une partie
                    rejouer = Console.ReadLine();   //Extraction de la réponse
                    
                } while (rejouer == "oui"); //Si la réponse est oui, le jeu reboot et tout recommence jusqu'ici

                Console.WriteLine("Merci d'avoir jouer! Jeu conçu par Gabriel Tarnus dans le cadre du travail de fin d'année du cours d'informatique de la classe 5TTI"); //Remerciement du jeu et credits
            }
            else   //Si la réponse n'a pas été "oui", alors...
            {
                Console.WriteLine("Ah, dommage, c'est le seul truc qu'on peut faire sur le programme pourtant");    //Message aux joueurs leur disant que c'était inutile de lancer le jeu alors
            }
           
                  
            Console.ReadLine(); //ReadLine pour ne pas quitter automatiquement le programme dès que ce dernier a fini de tourner
        }

        static void CreationEtui(ref string[] etui, ref Carte[] jeu) //Fonction qui va créer l'étui et y ranger les cartes
        {
            Random rnd = new Random();          //Random qui va permettre de sélectionner la carte aléatoirement
            int valeur;                         //Entier qui va conserver la valeur de rnd à chaque utilisation

            valeur = rnd.Next(0, 9);           //Random entre 0 et 8 pour choisir la piece du meurtre + conservation dans valeur
            etui[0] = jeu[valeur].carteName;    //Rangement de la carte dans l'étui
            jeu[valeur].distribue = true;       //Modification de distribue pour ne pas distribuer la carte aux joueurs par accident
            jeu[valeur].joueur = -1;            // -1 = étui

            valeur = rnd.Next(9, 15);          //Random entre 9 et 14 pour choisir l'arme du crime
            etui[1] = jeu[valeur].carteName;    //Rangement de la carte dans l'étui
            jeu[valeur].distribue = true;       //Modification de distribue pour ne pas distribuer la carte aux joueurs par accident
            jeu[valeur].joueur = -1;

            valeur = rnd.Next(15, 21);          //Random entre 15 et 20 pour choisir le meutrier du crime
            etui[2] = jeu[valeur].carteName;    //Rangement de la carte dans l'étui
            jeu[valeur].distribue = true;       //Modification de distribue pour ne pas distribuer la carte aux joueurs par accident
            jeu[valeur].joueur = -1;
        }

        static void Distribution(ref Carte[] jeu)   //Fonction qui va distribuer les cartes restantes aux 6 joueurs de manières complètement aléatoire
        {
            Random rnd = new Random();  //Random pour choisir la carte aléatoirement
            int valeur;                 //Entier qui contiendra le résultat de rnd
            bool carteDistrib;          //Bool qui permettra d'empêcher un verrou infini dans la boucle while, chose du par $joueur.distribue

            for(int joueur = 0; joueur < 3; joueur++)   //Boucle permettant de distribuer à chaque joueur leurs cartes
            {
                for(int carte = 0; carte < 6; carte++)  //Boucle qui doit faire distribuer 6 cartes
                {
                    do
                    {
                        valeur = rnd.Next(0, 21);               //Sélection du nombre aléatoire entre 0 et 21 pour choisir l'indice de la carte
                        carteDistrib = jeu[valeur].distribue;   //Affectation du booléen de $joueur.distribue à $carteDistrib
                        if (carteDistrib == false)              //Condition qui empêchera de distribuer deux fois la même carte
                        {
                            jeu[valeur].joueur = joueur;        //Insertion du numéro du joueur qui à la carte dans la carte
                            jeu[valeur].distribue = true;       //Indication disant que la carte a été distribuée
                        }
                        
                    } while (carteDistrib == true);     //Tant que $carteDistrib == true, rester dans "do"
                }  
            }
        }

        static void PlacementCartes(Carte[] jeu, ref int[,] cardJ) //Fonction qui va placer les ids de toutes les cartes à leurs joueurs réspectifs
        {
            int[] carteCur = new int[3] { 0, 0, 0 };    //Contient le nombre de cartes déjà distribué à un joueur

            for(int carte = 0; carte < jeu.Length; carte++)     //Boucle pour traiter chaque ligne du tableau en 2d
            {
                if(jeu[carte].joueur != -1 )    //Faire l'action principale de la fonction SI le joueur de la carte n'est pas l'étui/-1
                {
                    cardJ[jeu[carte].joueur,carteCur[jeu[carte].joueur]] = carte;   //Chercher la ligne en fonction du numéro du joueur (si joueur 2 --> ligne3) et la colonne en fonction du nombre de carte distribué au joueur x pour la decaler en fonction de ce dernier
                    carteCur[jeu[carte].joueur]++;  //Incrémentation du nombre de carte donner au joueur x de 1
                }
                
            }
        }

        static void FichesDeJeu(ref string[,,] fiches, Carte[] jeu) //Fonction qui va créer les fiches listants les 21 cartes pour que les joueurs puissent enquêter
        {
            for (int i = 0; i < fiches.GetLength(0); i++)   //Boucle allant de 0 à 2 pour pouvoir faire 3 fiches car il y a 3 joueurs
            {
                for(int j=0; j < fiches.GetLength(1); j++)  //Boucle allant de 0 à 20 pour traiter chaque cellule de $jeu
                {                  
                    fiches[i, j, 0] = jeu[j].carteName;     //Placement de la valeur de la carte dans la fiche
                    fiches[i, j, 1] = "";                
                }
            }
        }

        static void ActuFicheDebut(ref string[, ,] fiches, string[,] affCartes) //Fonction qui va permettre d'actualiser la fiche des joueurs dès le début de la partie avec les cartes qu'ils possèdent déjà
        {
            for(int i = 0; i<fiches.GetLength(0); i++)  //Compteur pour traverser chaque fiches
            {
                for(int j = 0; j < fiches.GetLength(1); j++)    //Compteur pour traverser chaque carte dans chaque fiche 
                {
                    for(int k = 0; k < affCartes.GetLength(1); k++) //Compteur pour traverser les colonnes de $affCartes
                    {
                        if(affCartes[i,k] == fiches[i,j,0])     //Si le contenu de tes cartes est égal à une quelquonque carte de ta fiche
                        {
                            fiches[i, j, 1] = "INNOCENT";   //La carte est innocente et donc éjecter de l'enquête
                        }
                    }
                }
            }
        }

        static void Hypotheses(Carte[] jeu, ref string[] hypothese, int nJoueur, ref string[] carteTrouve, string[,,] fiches)   //Fonction qui va permetttre aux joueurs 1 par 1 de proposer une hypothèse pour pouvoir avancer dans leur enquête
        {
            string refaire = "";    //Variable locale pour la situation où un joueur souhaiterait annuler ce qu'il a écrit
            do     //Boucle jusqu'à ce que
            {
                Console.WriteLine("Proposez un lieu du crime");
                hypothese[0] = Console.ReadLine();  //Insertion du lieu du crime entré par l'utilisateur

                Console.WriteLine("Proposez une arme du crime");
                hypothese[1] = Console.ReadLine();  //Insertion de l'arme du crime entrée par l'utilisateur

                Console.WriteLine("Proposez un suspect");
                hypothese[2] = Console.ReadLine();  //Insertion du meutrier entré par l'utilisateur

                Console.WriteLine("Confirmez-vous votre hypothèse??");  //Demande de confirmation des valeurs entrées par l'utilisateur
                refaire = Console.ReadLine();   //Insertion de la valeur
            } while (refaire != "oui"); //Si le joueur a marqué "oui" dans $refaire, alors la boucle recommencera

            TraitementHypothese(jeu, hypothese, nJoueur, ref carteTrouve);  //Appel de la fonction qui sert à traiter l'hypothèse

            ActuFicheHypothese(ref fiches, carteTrouve, nJoueur);

            for (int i = 0; i < carteTrouve.Length; i++) //Compteur pour parcourir $carteTrouve[]
            {
                carteTrouve[i] = "";    //Vidage de $carteTrouve[]
            }
        }

        static void TraitementHypothese(Carte[] jeu, string[] hypothese, int nJoueur, ref string[] carteTrouve) //Fonction qui va traiter l'hypothèse : si la carte x est détenu par le joueur y, le joueur y montre au joueur qui a rentré l'hypothèse
        {
            bool[] devoile = new bool[3] { false, false, false };   //Booléen pour chaque joueur de sorte que si un joueur a deux cartes à dévoiler, il ne pourra qu'en montrer une

            for(int i = 0; i<hypothese.Length; i++) //Compteur i pour parcourir chaque indice de $hypothese[]
            {
                int j = 0;  //Compteur pour parcourir $carteTrouve[]

                for(int k = 0; k < jeu.Length; k++) //Compteur pour parcourir les cartes de $jeu[]
                {
                    if(hypothese[i].ToUpper() == jeu[k].carteName.ToUpper())    //Si la valeur de la colonne d'indice $i de $hypothese[] est égal à la valeur du nom de la carte l'indice $k de $jeu[], alors... 
                    {
                        if(jeu[k].joueur != -1) //Si le numéro du joueur de l'indice $k de $jeu[] n'est pas -1 (l'étui), alors...
                        {
                            if(devoile[jeu[k].joueur] == false) //Si le booléen du joueur noté dans l'indice $k de $jeu[] est faux/pas encore utilisé, alors...
                            {
                                Console.WriteLine("Le joueur " + (jeu[k].joueur + 1) + " vous a révélé " + jeu[k].carteName);   //Message disant que le joueur x révèle au joueur entrant l'hypothèse une carte que ce dernier à noté dans $hypothese[]
                                Console.ReadLine(); //Console.ReadLine pour que le joueur puisse lire le message ci-dessus
                                
                                carteTrouve[j] = jeu[k].carteName;  //$carteTrouve prend le nom de la carte révéler pour pouvoir actualiser la fiche de jeu dans la prochaine fonction
                                j++;    //Incrémentation de $j
                                
                                devoile[jeu[k].joueur] = true;  //Le joueur x a montré une carte, maintenant il ne pourra plus
                            }                           
                        }
                    }
                }
            }
        }

        static void ActuFicheHypothese(ref string[,,] fiches, string[] carteTrouve, int nJoueur)    //Fonction qui va modifier la fiche du joueur et rendre innocent la/les carte(s) qu'il a découvert pour continuer son enquête
        {        
            for(int j =0; j < carteTrouve.Length; j++)  //Compteur pour parcourir chaque indice de $carteTrouve[] 
            {
                for (int k = 0; k < fiches.GetLength(1); k++)   //Compteur pour parcourir chaque carte de la fiche du joueur qui a entré l'hypothèse
                {
                    if (fiches[nJoueur, k, 0] == carteTrouve[j])   //Si la carte d'indice $k de la fiche de $nJoueur/le joueur ayant rentré l'hypothèse est égal à la valeur de l'indice $j dans carteTrouve[], alors...
                    {
                        fiches[nJoueur, k, 1] = "INNOCENT"; //La carte est innocente si la condition est vraie
                    }
                }
            }
        }

        static void Denonciation(int nJoueur, ref string[] denonciation, bool[] elimine, string[] etui, bool[] gagne, ref int comptePerd)   //Fonction qui va traiter la dénonciation du joueur : il va accuser ce qu'il juge coupable une bonne fois pour toute. Si il a raison il gagne, mais si c'est l'inverse il est éléminé du jeu
        {
            string refaire = "";    //String locale pour permettre au joueur de pouvoir réécrire sa dénonciation si il le souhaite
            do
            {
                Console.WriteLine("Proposez un lieu du crime"); //Demande d'insertion du lieu du crime
                denonciation[0] = Console.ReadLine();   //Extraction du résultat

                Console.WriteLine("Proposez une arme du crime");    //Demande d'insertion de l'arme du crime
                denonciation[1] = Console.ReadLine();   //Extraction du résultat

                Console.WriteLine("Proposez un suspect");   //Demande d'insertion du suspect
                denonciation[2] = Console.ReadLine();   //Extraction du résultat

                Console.WriteLine("Valider vous cette dénonciation? TOUTE ERREUR PEUT VOUS ÉLÉMINER DE LA PARTIE!!");   //Demande de confirmation des données entrées par le joueur
                refaire = Console.ReadLine();   //Extraction du résultat

            } while (refaire != "oui"); //Si l'utilisateur veut refaire sa dénonciation, la boucle remonte en arrière jusqu'ici
            
            VerifDenonciation(nJoueur, denonciation, ref elimine, etui, ref gagne); //Appel de la fonction permettant de savoir si le joueur a correctement deviné

            if(gagne[nJoueur] == true)  //Si le joueur a gagné, alors...
            {
                Console.WriteLine("Félicitations " + (nJoueur + 1) + ", vous avez gagné la partie!!!!");    //Message de félicitations pour le joueur qui a gagné
            }
            else   //Sinon...
            {
                if(elemine[nJoueur] == true)    //Si le joueur est éléminé, alors...
                {
                    Console.WriteLine("Désolé " + (nJoueur + 1) + ", mais vous vous êtes trompez. Par conséquent vous êtes éléminé de la partie."); //Message indiquant au joueur qu'ik est expédié de la partie
                    Console.ReadLine(); //ReadLine pour que le joueur puisse lire le message à son aise
                    comptePerd++;   //Le nombre de perdant augmente
                }
            }
        }

        static void VerifDenonciation(int nJoueur, string[] denonciation, ref bool[] elimine, string[] etui, ref bool[] gagne)  //Fonction qui va vérifier si la dénonciation du joueur est juste pour déterminé si il a gagné ou si il est éléminé
        {
            int gagneCompt = 0;

            bool[] trouve = new bool[3] { false, false, false };    //Bool pour noter pour chaque carte entré par l'utilisateur pour la dénonciation si elle est correcte
            for(int i = 0; i < denonciation.Length; i++)    //Compteur pour parcourir $denonciation[] et $etui[]
            {
                if(denonciation[i] == etui[i])  //Si la denonciation est correct avec l'étui
                {
                    trouve[i] = true;   //La carte est bonne
                }
            }

            for(int j = 0; j < denonciation.Length; j++)    //Compteur pour parcourir $gagne[] et $trouve[]
            {
                if(gagne[j] == false)   //Si le joueur n'a pas encore gagné, alors...
                {
                    if (trouve[j] == true)  //Si ce qu'il a trouvé est juste
                    {
                        gagneCompt++;   //Le nombre de cartes correct augmente
                        
                    }
                    else   //Si une carte est fausse, alors...
                    {
                        elimine[nJoueur] = true;    //Le joueur est éléminé de la partie
                    }
                }              
                if(gagneCompt == 3) //Si les trois cartes sont correct, alors...
                {
                    gagne[nJoueur] = true;  //Le joueur a gagné
                }
            }
        }

        static void AffichagePlacement(ref string[,] affCartes, int[,] cardJ, Carte[] jeu)   //Fonction qui va pouvoir préparer les futurs affichages des cartes des joueurs 
        {
            for(int i = 0; i < affCartes.GetLength(0); i++)   //Boucle allant de 0 à 2 car il y a 3 joueurs
            {
                for(int j = 0; j < cardJ.GetLength(1); j++)     //Boucle allant de 0 à 5 car il y a 6 cartes par joueur
                {
                    affCartes[i,j] = jeu[cardJ[i,j]].carteName; //Implentation des noms des cartes des joueurs pour les futurs traitements et affichages
                }
            }         
        }

        static void AfficheFiches(ref string[] afFiche, string[,,] fiches) //Fonction qui va pouvoir préparer les futurs affichages des fiches de jeu des joueurs 
        {
            string[] tempoFiches = new string[3];   //Variable où les fiches seront enregistrés avant d'aller dans la variable officielle pour les changements lors du jeu

            for (int j = 0; j < afFiche.Length; j++)     //Boucle allant de 0 à 2 pour traiter les fiches de chaque joueur
            {
                for (int i = 0; i < fiches.GetLength(1); i++)   //Boucle allant de 0 à 20 pour traiter chaque et les placer 1 par 1
                {
                    for(int k = 0; k < fiches.GetLength(2); k++)    //Boucle allant de 0 à 1 pour traiter les cartes et le message d'innocence
                    {
                        tempoFiches[j] = tempoFiches[j] + " "  +fiches[j, i, k].PadRight(30);      //Concaténation de $afFiche avec chaque carte + alignement de la deuxième colonne de $fiches dans la variable qui affiche       
                    }
                    tempoFiches[j] = tempoFiches[j] + "\n";     //Passer la ligne

                    switch (i)  //Switch pour améliorer l'affichage en séparent les types de cartes
                    {
                        case 8:                                         //Une fois que tous les lieux ont été placés
                            tempoFiches[j] = tempoFiches[j] + "\n";     //Passer la ligne
                            break;                                      //Briser le cas

                        case 14:                                        //Une fois que toutes les armes ont été placées
                            tempoFiches[j] = tempoFiches[j] + "\n";     //Passer la ligne
                            break;                                      //Briser le cas
                    }
                }
            }
            
            for(int l = 0; l < afFiche.Length; l++) //Compteur parcourant $afFiche[] et $tempoFiches[]
            {
                afFiche[l] = tempoFiches[l];    //Implantation des fiches dans leur variable officielle
            }
        }
    }
}