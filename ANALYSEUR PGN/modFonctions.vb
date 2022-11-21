Imports System.Drawing.Imaging
Imports System.IO
Imports System.Management 'ajouter référence system.management
Imports VB = Microsoft.VisualBasic

Module modFonctions

    Public moteur1 As New System.Diagnostics.Process()
    Public entree1 As System.IO.StreamWriter
    Public sortie1 As String
    Public erreur1 As System.IO.StreamReader

    Public moteur2 As New System.Diagnostics.Process()
    Public entree2 As System.IO.StreamWriter
    Public sortie2 As String
    Public erreur2 As System.IO.StreamReader

    Public tabPositions() As String
    Public tabPGN() As String
    Public tabEPD() As String
    Public pos As Integer
    Public idem As Integer
    Public mode As String

    Public Function analyseCoups(coupsEN As String, pgnFR As String, Optional epd As String = "") As String
        Dim tabCoups() As String, i As Integer, pieceCoups As String, separateur As String, promotion As String
        Dim chaine As String, departCoups As String, arriveeCoups As String, partie As String
        Dim departChaine As String, arriveeChaine As String, pieceChaine As String, chaineCoups As String
        Dim casesOccupees As String, prisePassant As String, coups As String, prefixFR As String

        chaineCoups = ""
        casesOccupees = "a1;b1;c1;d1;e1;f1;g1;h1;a2;b2;c2;d2;e2;f2;g2;h2;a7;b7;c7;d7;e7;f7;g7;h7;a8;b8;c8;d8;e8;f8;g8;h8;"
        prefixFR = "Ta1-a1 Cb1-b1 Fc1-c1 Dd1-d1 Re1-e1 Ff1-f1 Cg1-g1 Th1-h1 a2-a2 b2-b2 c2-c2 d2-d2 e2-e2 f2-f2 g2-g2 h2-h2 a7-a7 b7-b7 c7-c7 d7-d7 e7-e7 f7-f7 g7-g7 h7-h7 Ta8-a8 Cb8-b8 Fc8-c8 Dd8-d8 Re8-e8 Ff8-f8 Cg8-g8 Th8-h8 "
        If epd <> "" Then
            tabCoups = Split(epdCasesOccupees(epd), ":")
            casesOccupees = tabCoups(0)
            prefixFR = tabCoups(1)
        End If
        prisePassant = "a5xb6 b5xa6 b5xc6 c5xb6 c5xd6 d5xc6 d5xe6 e5xd6 e5xf6 f5xe6 f5xg6 g5xf6 g5xh6 h5xg6"
        prisePassant = prisePassant & " " & "a4xb3 b4xa3 b4xc3 c4xb3 c4xd3 d4xc3 d4xe3 e4xd3 e4xf3 f4xe3 f4xg3 g4xf3 g4xh3 h4xg3"
        pieceCoups = ""
        separateur = ""
        departCoups = ""
        arriveeCoups = ""
        departChaine = ""
        arriveeChaine = ""
        pieceChaine = ""
        promotion = ""

        'on dégage les commentaires
        partie = formaterCoups("", prefixFR & pgnFR)

        'on convertit les roques
        If InStr(partie, ". 0-0 ", CompareMethod.Text) > 0 Then
            partie = Replace(partie, ". 0-0 ", ". Re1-g1 Th1-f1 ")
        ElseIf InStr(partie, ". 0-0-0 ", CompareMethod.Text) > 0 Then
            partie = Replace(partie, ". 0-0-0 ", ". Re1-c1 Ta1-d1 ")
        End If
        If InStr(partie, " 0-0 ", CompareMethod.Text) > 0 Then
            partie = Replace(partie, " 0-0 ", " Re8-g8 Th8-f8 ")
        ElseIf InStr(partie, " 0-0-0 ", CompareMethod.Text) > 0 Then
            partie = Replace(partie, " 0-0-0 ", " Re8-c8 Ta8-d8 ")
        End If

        'on convertit les promotions
        'h2-h4 f7-f6 h4-h5 f6-f5 h5-h6 f5-f4 h6xg7 f4-f3 g7xh8=D Ff8-g7
        'g7xh8=D => g7xh8 + Dh8-h8
        While InStr(partie, "=", CompareMethod.Text) > 0
            partie = Replace(partie, partie.Substring(partie.IndexOf("=") - 2, 4), partie.Substring(partie.IndexOf("=") - 2, 2) & " " & partie.Substring(partie.IndexOf("=") + 1, 1) & partie.Substring(partie.IndexOf("=") - 2, 2) & "-" & partie.Substring(partie.IndexOf("=") - 2, 2))
            Application.DoEvents()
        End While

        'on cherche les infos du coups
        coups = formaterCoups("moteur", coupsEN)
        departCoups = gauche(coups, 2)
        If Len(coups) < 5 Then
            arriveeCoups = droite(coups, 2)
        Else
            Select Case droite(coups, 1)
                Case "q", "Q"
                    promotion = "=D"
                Case "r", "R"
                    promotion = "=T"
                Case "b", "B"
                    promotion = "=F"
                Case "n", "N"
                    promotion = "=C"
            End Select
            arriveeCoups = droite(gauche(coups, 4), 2)
        End If

        'on met à jour les cases occupées
        tabCoups = Split(partie, " ")
        For i = 0 To UBound(tabCoups)
            If tabCoups(i) <> "" And InStr(tabCoups(i), ".", CompareMethod.Text) = 0 And InStr(tabCoups(i), "{", CompareMethod.Text) = 0 And InStr(tabCoups(i), "}", CompareMethod.Text) = 0 And InStr(tabCoups(i), "/", CompareMethod.Text) = 0 _
            And InStr(tabCoups(i), "*", CompareMethod.Text) = 0 And InStr(tabCoups(i), "1-0", CompareMethod.Text) = 0 And InStr(tabCoups(i), "0-1", CompareMethod.Text) = 0 And InStr(tabCoups(i), "1/2-1/2", CompareMethod.Text) = 0 Then
                'si on a une prise en passant
                If InStr(tabCoups(i), "ep", CompareMethod.Text) > 0 Then
                    'on efface la case d'arrivée du coups précédent (pion qui avance de 2 cases)
                    casesOccupees = Replace(casesOccupees, arriveeChaine & ";", "")
                End If
                chaine = formaterCoups("moteur", tabCoups(i))

                departChaine = gauche(chaine, 2)
                arriveeChaine = droite(chaine, 2)

                casesOccupees = Replace(casesOccupees, departChaine & ";", "")
                If InStr(casesOccupees, arriveeChaine & ";") = 0 Then
                    casesOccupees = casesOccupees & arriveeChaine & ";"
                End If

                casesOccupees = trierChaine(casesOccupees, ";") & ";"

            End If
            Application.DoEvents()
        Next

        departChaine = ""
        arriveeChaine = ""

        'on analyse le coups
        For i = UBound(tabCoups) To 0 Step -1
            If tabCoups(i) <> "" And InStr(tabCoups(i), ".", CompareMethod.Text) = 0 And InStr(tabCoups(i), "{", CompareMethod.Text) = 0 And InStr(tabCoups(i), "}", CompareMethod.Text) = 0 And InStr(tabCoups(i), "/", CompareMethod.Text) = 0 _
            And InStr(tabCoups(i), "*", CompareMethod.Text) = 0 And InStr(tabCoups(i), "1-0", CompareMethod.Text) = 0 And InStr(tabCoups(i), "0-1", CompareMethod.Text) = 0 And InStr(tabCoups(i), "1/2-1/2", CompareMethod.Text) = 0 Then
                pieceChaine = ""

                chaine = tabCoups(i)
                If Len(tabCoups(i)) = 6 Then
                    chaine = droite(tabCoups(i), 5)
                    pieceChaine = gauche(tabCoups(i), 1)
                End If

                departChaine = gauche(chaine, 2)
                arriveeChaine = droite(chaine, 2)

                'case de départ connue ?
                If pieceCoups = "" And departCoups = arriveeChaine Then
                    If pieceChaine = "" Then
                        pieceCoups = "p"
                    Else
                        pieceCoups = pieceChaine
                    End If
                End If

                'case d'arrivée connue ?
                If separateur = "" And InStr(casesOccupees, arriveeCoups & ";") > 0 Then
                    separateur = "x"
                End If
            End If
        Next

        If separateur = "" Then
            separateur = "-"
        End If

        chaineCoups = Replace(pieceCoups, "p", "") & departCoups & separateur & arriveeCoups & promotion

        'roque
        If chaineCoups = "Re1-g1" Or chaineCoups = "Re8-g8" Then
            chaineCoups = "0-0"
        ElseIf chaineCoups = "Re1-c1" Or chaineCoups = "Re8-c8" Then
            chaineCoups = "0-0-0"
        End If

        'prise en passant
        If InStr(prisePassant, Replace(chaineCoups, "-", "x")) > 0 Then
            chaineCoups = Replace(chaineCoups, "-", "x")
        End If

        Return chaineCoups
    End Function

    Public Function binLong(suite As String) As Long
        Dim i As Integer, cumul As Long

        cumul = 0
        For i = 1 To Len(suite)
            cumul = cumul + CInt(gauche(droite(suite, i), 1)) * 2 ^ (i - 1)
        Next
        Return cumul

    End Function

    Public Sub captureEcran(fenetre As System.Windows.Forms.Form, Optional lieu As String = "capture.png")
        'indiquer nothing dans fenetre pour capturer le bureau
        Try
            If fenetre Is Nothing Then
                Dim largeur As Integer, hauteur As Integer
                largeur = Screen.PrimaryScreen.Bounds.Width
                If largeur < 1920 Then
                    largeur = 1920
                End If
                hauteur = Screen.PrimaryScreen.Bounds.Height
                If hauteur < 1080 Then
                    hauteur = 1080
                End If

                Dim image = New Bitmap(largeur, hauteur)

                Graphics.FromImage(image).CopyFromScreen(New Point(0, 0), New Point(0, 0), New System.Drawing.Size(largeur, hauteur))
                Graphics.FromImage(image).Flush()

                image.Save(lieu, ImageFormat.Png)
            Else
                Dim image = New Bitmap(fenetre.Width, fenetre.Height)

                fenetre.DrawToBitmap(image, New Rectangle(0, 0, fenetre.Width, fenetre.Height))

                image.Save(lieu, ImageFormat.Png)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Sub chargerMoteur1(chemin As String, priorite As String, puissance As String, memoire As String, contempt As String)
        Dim i As Integer, chaine As String, tabChaine() As String, options1 As String

        sortie1 = ""

        'chargement moteur #1
        moteur1.StartInfo.UseShellExecute = False
        moteur1.StartInfo.RedirectStandardOutput = True
        AddHandler moteur1.OutputDataReceived, AddressOf evenement1
        moteur1.StartInfo.RedirectStandardInput = True
        moteur1.StartInfo.RedirectStandardError = True
        moteur1.StartInfo.CreateNoWindow = True
        If My.Computer.FileSystem.DirectoryExists(My.Computer.FileSystem.GetParentPath(chemin)) Then
            moteur1.StartInfo.WorkingDirectory = My.Computer.FileSystem.GetParentPath(chemin)
        Else
            moteur1.StartInfo.WorkingDirectory = Application.StartupPath
        End If
        If InStr(chemin, "mpiexec.exe", CompareMethod.Text) > 0 Then
            moteur1.StartInfo.FileName = "mpiexec.exe"
            moteur1.StartInfo.Arguments = chemin.Substring(InStr(chemin, "mpiexec.exe", CompareMethod.Text) + Len("mpiexec.exe"))
        Else
            moteur1.StartInfo.FileName = ("""" & chemin & """")
        End If
        moteur1.Start()
        If priorite = "normal" Then
            moteur1.PriorityClass = ProcessPriorityClass.Normal
        ElseIf priorite = "below" Then
            moteur1.PriorityClass = ProcessPriorityClass.BelowNormal
        ElseIf priorite = "idle" Then
            moteur1.PriorityClass = ProcessPriorityClass.Idle
        End If

        entree1 = moteur1.StandardInput
        erreur1 = moteur1.StandardError

        entree1.WriteLine("uci")
        moteur1.BeginOutputReadLine()

        While InStr(sortie1, "uciok", CompareMethod.Text) = 0
            Application.DoEvents()
        End While
        sortie1 = ""

        'options communes
        If InStr(chemin, "rybka", CompareMethod.Text) > 0 Then
            entree1.WriteLine("setoption name Max CPUs value " & puissance)
        Else
            entree1.WriteLine("setoption name Threads value " & puissance)
        End If
        entree1.WriteLine("setoption name Ponder value false")
        entree1.WriteLine("setoption name MultiPV value 1")
        entree1.WriteLine("setoption name Hash value " & memoire)
        entree1.WriteLine("setoption name Contempt value " & contempt)

        'options spécifiques
        If InStr(chemin, "mpiexec.exe", CompareMethod.Text) > 0 Then
            options1 = Replace(chemin.Substring(chemin.LastIndexOf(" ") + 1), ".exe", ".txt")
        Else
            options1 = Replace(chemin.Substring(chemin.LastIndexOf("\") + 1), ".exe", ".txt")
        End If
        If My.Computer.FileSystem.FileExists(options1) Then
            chaine = My.Computer.FileSystem.ReadAllText(options1)
            tabChaine = Split(chaine, vbCrLf)
            For i = 0 To UBound(tabChaine)
                If tabChaine(i) <> "" Then
                    entree1.WriteLine(tabChaine(i))
                End If
                Application.DoEvents()
            Next
        End If

        'synchronisation
        entree1.WriteLine("isready")
        Do
            If InStr(sortie1, "tablebase", CompareMethod.Text) > 0 Then
                tabChaine = Split(sortie1, vbCrLf)
                For i = 0 To UBound(tabChaine)
                    If InStr(tabChaine(i), "info string found ", CompareMethod.Text) > 0 _
                    And InStr(tabChaine(i), " tablebase", CompareMethod.Text) > 0 _
                    And InStr(tabChaine(i), " in ", CompareMethod.Text) = 0 Then
                        chaine = Replace(LCase(tabChaine(i)), "info string found ", "")
                        frmPrincipale.lblTB1.Text = chaine.Substring(0, chaine.IndexOf(" ")) & " tablebases"
                        Exit For
                    End If
                Next
            End If
            Application.DoEvents()
        Loop While InStr(sortie1, "readyok", CompareMethod.Text) = 0
        sortie1 = ""
    End Sub

    Public Sub chargerMoteur2(chemin As String, priorite As String, puissance As String, memoire As String, contempt As String)
        Dim i As Integer, chaine As String, tabChaine() As String, options2 As String
        Dim affinite1 As Long, affinite2 As Long

        sortie2 = ""

        'chargement moteur #2
        moteur2.StartInfo.UseShellExecute = False
        moteur2.StartInfo.RedirectStandardOutput = True
        AddHandler moteur2.OutputDataReceived, AddressOf evenement2
        moteur2.StartInfo.RedirectStandardInput = True
        moteur2.StartInfo.RedirectStandardError = True
        moteur2.StartInfo.CreateNoWindow = True
        moteur2.StartInfo.WorkingDirectory = My.Computer.FileSystem.GetParentPath(chemin)
        moteur2.StartInfo.FileName = ("""" & chemin & """")
        moteur2.Start()
        If priorite = "normal" Then
            moteur2.PriorityClass = ProcessPriorityClass.Normal
        ElseIf priorite = "below" Then
            moteur2.PriorityClass = ProcessPriorityClass.BelowNormal
        ElseIf priorite = "idle" Then
            moteur2.PriorityClass = ProcessPriorityClass.Idle
        End If

        entree2 = moteur2.StandardInput
        erreur2 = moteur2.StandardError

        entree2.WriteLine("uci")
        moteur2.BeginOutputReadLine()

        While InStr(sortie2, "uciok", CompareMethod.Text) = 0
            Application.DoEvents()
        End While
        sortie2 = ""

        'options communes
        If InStr(chemin, "rybka", CompareMethod.Text) > 0 Then
            entree2.WriteLine("setoption name Max CPUs value " & puissance)
        Else
            entree2.WriteLine("setoption name Threads value " & puissance)
        End If
        entree2.WriteLine("setoption name Ponder value false")
        entree2.WriteLine("setoption name MultiPV value 1")
        entree2.WriteLine("setoption name Hash value " & memoire)
        entree2.WriteLine("setoption name Contempt value " & contempt)

        'options spécifiques
        options2 = Replace(chemin.Substring(chemin.LastIndexOf("\") + 1), ".exe", ".txt")
        If My.Computer.FileSystem.FileExists(options2) Then
            chaine = My.Computer.FileSystem.ReadAllText(options2)
            tabChaine = Split(chaine, vbCrLf)
            For i = 0 To UBound(tabChaine)
                If tabChaine(i) <> "" Then
                    entree2.WriteLine(tabChaine(i))
                End If
                Application.DoEvents()
            Next
        End If

        'synchronisation
        entree2.WriteLine("isready")
        Do
            If InStr(sortie2, "tablebase", CompareMethod.Text) > 0 Then
                tabChaine = Split(sortie2, vbCrLf)
                For i = 0 To UBound(tabChaine)
                    If InStr(tabChaine(i), "info string found ", CompareMethod.Text) > 0 _
                    And InStr(tabChaine(i), " tablebase", CompareMethod.Text) > 0 _
                    And InStr(tabChaine(i), " in ", CompareMethod.Text) = 0 Then
                        chaine = Replace(LCase(tabChaine(i)), "info string found ", "")
                        frmPrincipale.lblTB2.Text = chaine.Substring(0, chaine.IndexOf(" ")) & " tablebases"
                        Exit For
                    End If
                Next
            End If
            Application.DoEvents()
        Loop While InStr(sortie2, "readyok", CompareMethod.Text) = 0
        sortie2 = ""

        'quand sugar est le moteur2 comme son affinité est en lecture seule
        'et qu'il utilise les threads dans l'ordre, il faut reconfigurer le moteur1
        'pour qu'il utilise d'autres threads
        affinite1 = binLong(StrDup(CInt(cpu() / 2), "0") & StrDup(CInt(cpu() / 2), "1"))
        affinite2 = binLong(StrDup(CInt(cpu() / 2), "1") & StrDup(CInt(cpu() / 2), "0"))
        '01-20 =       1048575 = 00000FFFFF
        '21-40 = 1099510579200 = FFFFF00000 = dépassement
        If InStr(chemin, "sugar", CompareMethod.Text) > 0 Then
            'moteur1 = xeon2
            'sugarnn = xeon1
            moteur1.ProcessorAffinity = New IntPtr(affinite2)
            moteur2.ProcessorAffinity = New IntPtr(affinite1)
        Else
            'moteur1 = xeon1
            'moteur2 = xeon2
            moteur1.ProcessorAffinity = New IntPtr(affinite1)
            moteur2.ProcessorAffinity = New IntPtr(affinite2)
        End If
    End Sub

    Public Function coupsEN(coups As String) As String
        Dim chaine As String

        chaine = Replace(coups, "F", "B")
        chaine = Replace(chaine, "C", "N")
        chaine = Replace(chaine, "D", "Q")
        chaine = Replace(chaine, "R", "K")
        chaine = Replace(chaine, "T", "R")

        Return chaine
    End Function

    Public Function coupsFR(coups As String) As String
        Dim chaine As String

        chaine = Replace(coups, "B", "F")
        chaine = Replace(chaine, "N", "C")
        chaine = Replace(chaine, "Q", "D")
        chaine = Replace(chaine, "R", "T")
        chaine = Replace(chaine, "K", "R")

        Return chaine
    End Function

    Public Function cpu(Optional reel As Boolean = False) As Integer
        Dim collection As New ManagementObjectSearcher("select * from Win32_Processor"), taches As Integer
        taches = 0

        For Each element As ManagementObject In collection.Get
            If reel Then
                taches = taches + element.Properties("NumberOfCores").Value 'cores
            Else
                taches = taches + element.Properties("NumberOfLogicalProcessors").Value 'threads
            End If
        Next

        Return taches
    End Function

    Public Function droite(texte As String, longueur As Integer) As String
        If longueur > 0 Then
            Return VB.Right(texte, longueur)
        Else
            Return ""
        End If
    End Function

    Public Sub echiquier(epd As String, e As PaintEventArgs, Optional coup1 As String = "", Optional coup2 As String = "")
        Dim tabEPD() As String, tabCaracteres() As Char
        Dim i As Integer, j As Integer, ligne As Integer, colonne As Integer
        Dim x As Integer, y As Integer, offsetGauche As Integer, offetHaut As Integer
        Dim largeur As Integer, hauteur As Integer, caseNoire As Boolean, chaine As String
        Dim trait As Char, stylo As System.Drawing.Pen, pinceau As System.Drawing.Brush
        Dim transparence As New ImageAttributes

        'minuscule = noirs
        'majuscule = blancs
        'w = trait blanc
        'b = trait noir
        '1-8 = cases vides

        'initialisation
        ligne = 1
        colonne = 1
        offsetGauche = -9
        offetHaut = -10
        largeur = 40 'grille
        hauteur = 40 'grille
        caseNoire = False 'en haut à gauche la case est blanche
        transparence.SetColorKey(Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 0))

        'formatage
        For i = 2 To 8
            epd = Replace(epd, i, StrDup(i, "1"))
        Next

        'qui a le trait
        trait = ""
        If InStr(epd, " w ") > 0 Or InStr(epd, " b ") > 0 Then
            trait = epd.Chars(epd.IndexOf(" ") + 1)
        End If

        'dessiner la position de base
        tabEPD = Split(epd, "/")
        For i = 0 To UBound(tabEPD)
            If tabEPD(i) <> "" Then
                tabCaracteres = tabEPD(i).ToCharArray
                For j = 0 To UBound(tabCaracteres)
                    If tabCaracteres(j) = " " Then
                        Exit For
                    End If

                    x = colonne * largeur + offsetGauche  'centre case
                    y = ligne * hauteur + offetHaut 'centre case

                    'pièces
                    chaine = ""
                    Select Case tabCaracteres(j)
                        Case "r" 'rook = tour
                            chaine = "tourN"

                        Case "R"
                            chaine = "tourB"

                        Case "n" 'knight = cavalier
                            chaine = "cavalierN"

                        Case "N"
                            chaine = "cavalierB"

                        Case "b" 'bishop = fou
                            chaine = "fouN"

                        Case "B"
                            chaine = "fouB"

                        Case "q" 'queen = dame
                            chaine = "dameN"

                        Case "Q"
                            chaine = "dameB"

                        Case "k" 'king = roi
                            chaine = "roiN"

                        Case "K"
                            chaine = "roiB"

                        Case "p" 'pawn = pion
                            chaine = "pionN"

                        Case "P"
                            chaine = "pionB"

                    End Select

                    If chaine <> "" Then
                        e.Graphics.DrawImage(Image.FromFile("bmp\" & chaine & ".bmp"), New Rectangle(x - 14, y - 12, 37, 37), 0, 0, 37, 37, GraphicsUnit.Pixel, transparence)
                    End If

                    caseNoire = Not caseNoire
                    colonne = colonne + 1
                    If colonne = 9 Then
                        caseNoire = Not caseNoire
                        colonne = 1
                        ligne = ligne + 1
                    End If
                Next
            End If
        Next

        'coup moteur 1
        If coup1 <> "" And Not IsNothing(coup1) Then
            'moteurs d'accord ?
            If (Not IsNothing(coup2) And coup1 <> coup2) Or IsNothing(coup2) Then
                'non
                stylo = Pens.Blue
                pinceau = Brushes.Blue
            Else
                'oui
                stylo = Pens.Red
                pinceau = Brushes.Red
            End If

            echiquier_localisation(e, pinceau, stylo, coup1, tabEPD, trait, largeur, hauteur, offsetGauche)
        End If

        'coup moteur 2
        If coup2 <> "" And Not IsNothing(coup2) And ((Not IsNothing(coup1) And coup2 <> coup1) Or IsNothing(coup1)) Then
            stylo = Pens.Green
            pinceau = Brushes.Green

            echiquier_localisation(e, pinceau, stylo, coup2, tabEPD, trait, largeur, hauteur, offsetGauche)
        End If

        'trait
        If trait = "w" Then
            e.Graphics.FillEllipse(Brushes.White, New Rectangle(e.ClipRectangle.Width - 19, e.ClipRectangle.Height - 20, 15, 15))
            e.Graphics.DrawEllipse(Pens.Black, New Rectangle(e.ClipRectangle.Width - 19, e.ClipRectangle.Height - 20, 15, 15))
        ElseIf trait = "b" Then
            e.Graphics.FillEllipse(Brushes.Black, New Rectangle(e.ClipRectangle.Width - 19, 1, 15, 15))
            e.Graphics.DrawEllipse(Pens.Black, New Rectangle(e.ClipRectangle.Width - 19, 1, 15, 15))
        End If

        'totaux pièces
        e.Graphics.DrawString(epdTotalNoir(epd), New Font("courier new", 8), Brushes.Black, New Point(0, 0))
        e.Graphics.DrawString(epdTotalBlanc(epd), New Font("courier new", 8), Brushes.Black, New Point(0, e.ClipRectangle.Height - 16))

    End Sub

    Public Sub echiquier_localisation(e As PaintEventArgs, pinceau As System.Drawing.Brush, stylo As System.Drawing.Pen, coupMoteur As String, tabEPD() As String, trait As Char, largeur As Integer, hauteur As Integer, offsetGauche As Integer)
        Dim offsetRoque As Integer, x As Integer, y As Integer, colonne As Integer, ligne As Integer, xTrait As Integer, yTrait As Integer

        'rook
        offsetRoque = 7
        If coupMoteur = "e1g1" And tabEPD(7).Chars(4) = "K" And tabEPD(7).Chars(7) = "R" And trait = "w" Then
            '0-0
            'e1 => g1
            x = Val(Asc(coupMoteur.Chars(0)) - 96) * largeur + offsetGauche + 4
            y = (9 - Val(coupMoteur.Chars(1))) * hauteur - 4
            colonne = Val(Asc(coupMoteur.Chars(2)) - 96) * largeur + offsetGauche + 4
            ligne = (9 - Val(coupMoteur.Chars(3))) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 + offsetRoque, x + largeur / 2, y + offsetRoque, colonne, ligne + offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

            'h1 => f1
            x = Val(Asc("h") - 96) * largeur + offsetGauche + 4
            y = (9 - 1) * hauteur - 4
            colonne = Val(Asc("f") - 96) * largeur + offsetGauche + 4
            ligne = (9 - 1) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 - offsetRoque, x - largeur / 2, y - offsetRoque, colonne, ligne - offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

        ElseIf coupMoteur = "e1c1" And tabEPD(7).Chars(4) = "K" And tabEPD(7).Chars(0) = "R" And trait = "w" Then
            '0-0-0
            'e1 => c1
            x = Val(Asc(coupMoteur.Chars(0)) - 96) * largeur + offsetGauche + 4
            y = (9 - Val(coupMoteur.Chars(1))) * hauteur - 4
            colonne = Val(Asc(coupMoteur.Chars(2)) - 96) * largeur + offsetGauche + 4
            ligne = (9 - Val(coupMoteur.Chars(3))) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 + offsetRoque, x - largeur / 2, y + offsetRoque, colonne, ligne + offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

            'a1 => d1
            x = Val(Asc("a") - 96) * largeur + offsetGauche + 4
            y = (9 - 1) * hauteur - 4
            colonne = Val(Asc("d") - 96) * largeur + offsetGauche + 4
            ligne = (9 - 1) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 - offsetRoque, x + largeur / 2, y - offsetRoque, colonne, ligne - offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

        ElseIf coupMoteur = "e8g8" And tabEPD(0).Chars(4) = "k" And tabEPD(0).Chars(7) = "r" And trait = "b" Then
            '0-0
            'e8 => g8
            x = Val(Asc(coupMoteur.Chars(0)) - 96) * largeur + offsetGauche + 4
            y = (9 - Val(coupMoteur.Chars(1))) * hauteur - 4
            colonne = Val(Asc(coupMoteur.Chars(2)) - 96) * largeur + offsetGauche + 4
            ligne = (9 - Val(coupMoteur.Chars(3))) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 + offsetRoque, x + largeur / 2, y + offsetRoque, colonne, ligne + offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

            'h8 => f8
            x = Val(Asc("h") - 96) * largeur + offsetGauche + 4
            y = (9 - 8) * hauteur - 4
            colonne = Val(Asc("f") - 96) * largeur + offsetGauche + 4
            ligne = (9 - 8) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 - offsetRoque, x - largeur / 2, y - offsetRoque, colonne, ligne - offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

        ElseIf coupMoteur = "e8c8" And tabEPD(0).Chars(4) = "k" And tabEPD(0).Chars(0) = "r" And trait = "b" Then
            '0-0-0
            'e8 => c8
            x = Val(Asc(coupMoteur.Chars(0)) - 96) * largeur + offsetGauche + 4
            y = (9 - Val(coupMoteur.Chars(1))) * hauteur - 4
            colonne = Val(Asc(coupMoteur.Chars(2)) - 96) * largeur + offsetGauche + 4
            ligne = (9 - Val(coupMoteur.Chars(3))) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 + offsetRoque, x - largeur / 2, y + offsetRoque, colonne, ligne + offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

            'a8 => d8
            x = Val(Asc("a") - 96) * largeur + offsetGauche + 4
            y = (9 - 8) * hauteur - 4
            colonne = Val(Asc("d") - 96) * largeur + offsetGauche + 4
            ligne = (9 - 8) * hauteur - 4
            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4 - offsetRoque, x + largeur / 2, y - offsetRoque, colonne, ligne - offsetRoque, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

        Else
            'coup simple
            x = Val(Asc(coupMoteur.Chars(0)) - 96) * largeur + offsetGauche + 4
            y = (9 - Val(coupMoteur.Chars(1))) * hauteur - 4
            colonne = Val(Asc(coupMoteur.Chars(2)) - 96) * largeur + offsetGauche + 4
            ligne = (9 - Val(coupMoteur.Chars(3))) * hauteur - 4
            xTrait = 0
            yTrait = 0
            offsetRoque = 0

            'traits
            If x = colonne Then
                'même colonne
                If y > ligne Then
                    'vers le haut
                    xTrait = x
                    yTrait = y - hauteur / 2
                Else
                    'vers le bas
                    xTrait = x
                    yTrait = y + hauteur / 2
                End If
            ElseIf y = ligne Then
                'même ligne
                If x < colonne Then
                    'vers la droite
                    xTrait = x + largeur / 2
                    yTrait = y
                Else
                    'vers la gauche
                    xTrait = x - largeur / 2
                    yTrait = y
                End If
            ElseIf colonne > x And ligne < y Then
                'vers le haut et vers la droite
                xTrait = x + largeur / 2
                yTrait = y - hauteur / 2
            ElseIf colonne < x And ligne < y Then
                'vers le haut et vers la gauche
                xTrait = x - largeur / 2
                yTrait = y - hauteur / 2
            ElseIf colonne > x And ligne > y Then
                'vers le bas et vers la droite
                xTrait = x + largeur / 2
                yTrait = y + hauteur / 2
            ElseIf colonne < x And ligne > y Then
                'vers le bas et vers la gauche
                xTrait = x - largeur / 2
                yTrait = y + hauteur / 2
            End If

            echiquier_dessin(e, pinceau, stylo, colonne - 4, ligne - 4, xTrait, yTrait, colonne, ligne, x - largeur / 2, y - hauteur / 2, largeur, hauteur)

        End If
    End Sub

    Public Sub echiquier_dessin(e As PaintEventArgs, pinceau As System.Drawing.Brush, stylo As System.Drawing.Pen, xRond As Integer, yRond As Integer, xTrait As Integer, yTrait As Integer, x1Trait As Integer, y1Trait As Integer, xContour As Integer, yContour As Integer, largContour As Integer, hautContour As Integer)
        'ronds
        e.Graphics.FillEllipse(pinceau, New Rectangle(xRond, yRond, 8, 8))

        'trait
        e.Graphics.DrawLine(stylo, New Point(xTrait, yTrait), New Point(x1Trait, y1Trait))

        'contours
        e.Graphics.DrawRectangle(stylo, New Rectangle(xContour, yContour, largContour, hautContour))
        e.Graphics.DrawRectangle(stylo, New Rectangle(xContour + 1, yContour + 1, largContour - 2, hautContour - 2))
        e.Graphics.DrawRectangle(stylo, New Rectangle(xContour + 2, yContour + 2, largContour - 4, hautContour - 4))

    End Sub

    Public Sub echiquier_differences(epd As String, e As PaintEventArgs)
        Dim i As Integer, piecesNoires As String, piecesBlanches As String, tmp() As Char
        Dim chaine As String, transparence As New ImageAttributes
        Dim pasN As Integer, pasB As Integer, larg As Integer

        larg = 20
        transparence.SetColorKey(Color.FromArgb(0, 255, 0), Color.FromArgb(0, 255, 0))

        'formatage
        For i = 1 To 8
            epd = Replace(epd, i, "")
        Next
        epd = Replace(epd, "/", "")
        If epd = "" Then
            Exit Sub
        End If
        epd = epd.Substring(0, epd.IndexOf(" "))

        'on liste les pièces noires
        piecesNoires = ""
        piecesBlanches = ""
        tmp = epd.ToCharArray
        For i = 0 To UBound(tmp)
            Select Case tmp(i)
                Case "r" 'rook = tour
                    piecesNoires = piecesNoires & tmp(i)

                Case "R"
                    piecesBlanches = piecesBlanches & tmp(i)

                Case "n" 'knight = cavalier
                    piecesNoires = piecesNoires & tmp(i)

                Case "N"
                    piecesBlanches = piecesBlanches & tmp(i)

                Case "b" 'bishop = fou
                    piecesNoires = piecesNoires & tmp(i)

                Case "B"
                    piecesBlanches = piecesBlanches & tmp(i)

                Case "q" 'queen = dame
                    piecesNoires = piecesNoires & tmp(i)

                Case "Q"
                    piecesBlanches = piecesBlanches & tmp(i)

                Case "p" 'pawn = pion
                    piecesNoires = piecesNoires & tmp(i)

                Case "P"
                    piecesBlanches = piecesBlanches & tmp(i)

            End Select
        Next

        'on supprime chaque pièce noir dans la liste des blanches
        tmp = piecesNoires.ToCharArray
        For i = 0 To UBound(tmp)
            If InStr(piecesBlanches, tmp(i), CompareMethod.Text) > 0 Then
                piecesBlanches = Replace(piecesBlanches, tmp(i), "", , 1, CompareMethod.Text)
                piecesNoires = Replace(piecesNoires, tmp(i), "", , 1)
            End If
        Next

        'on trie les pièces par valeur
        If piecesBlanches <> "" Then
            piecesBlanches = Replace(piecesBlanches, "Q", "1")
            piecesBlanches = Replace(piecesBlanches, "R", "2")
            piecesBlanches = Replace(piecesBlanches, "B", "3")
            piecesBlanches = Replace(piecesBlanches, "N", "4")
            piecesBlanches = Replace(piecesBlanches, "P", "5")
            'chaine vers tableau => tri alphabétique => tableau vers chaine
            tmp = piecesBlanches.ToCharArray
            Array.Sort(tmp)
            piecesBlanches = New String(tmp)
            'on met en forme la chaine
            piecesBlanches = Replace(piecesBlanches, "1", "Q")
            piecesBlanches = Replace(piecesBlanches, "2", "R")
            piecesBlanches = Replace(piecesBlanches, "3", "B")
            piecesBlanches = Replace(piecesBlanches, "4", "N")
            piecesBlanches = Replace(piecesBlanches, "5", "P")
        End If

        'on trie les pièces par valeur
        If piecesNoires <> "" Then
            piecesNoires = Replace(piecesNoires, "q", "1")
            piecesNoires = Replace(piecesNoires, "r", "2")
            piecesNoires = Replace(piecesNoires, "b", "3")
            piecesNoires = Replace(piecesNoires, "n", "4")
            piecesNoires = Replace(piecesNoires, "p", "5")
            'chaine vers tableau => tri alphabétique => tableau vers chaine
            tmp = piecesNoires.ToCharArray
            Array.Sort(tmp)
            piecesNoires = New String(tmp)
            'on met en forme la chaine
            piecesNoires = Replace(piecesNoires, "1", "q")
            piecesNoires = Replace(piecesNoires, "2", "r")
            piecesNoires = Replace(piecesNoires, "3", "b")
            piecesNoires = Replace(piecesNoires, "4", "n")
            piecesNoires = Replace(piecesNoires, "5", "p")
        End If

        'on cumule les pièces manquantes (noires à gauche, blanches à droite)
        chaine = piecesNoires & piecesBlanches

        If chaine = "" Then
            Exit Sub
        End If

        'pièces
        tmp = chaine.ToCharArray
        pasN = -1
        pasB = -1
        For i = 0 To UBound(tmp)
            Select Case tmp(i)
                Case "r" 'rook = tour
                    chaine = "tourN"

                Case "R"
                    chaine = "tourB"

                Case "n" 'knight = cavalier
                    chaine = "cavalierN"

                Case "N"
                    chaine = "cavalierB"

                Case "b" 'bishop = fou
                    chaine = "fouN"

                Case "B"
                    chaine = "fouB"

                Case "q" 'queen = dame
                    chaine = "dameN"

                Case "Q"
                    chaine = "dameB"

                Case "p" 'pawn = pion
                    chaine = "pionN"

                Case "P"
                    chaine = "pionB"

            End Select

            If chaine <> "" Then
                If InStr(chaine, "N") = Len(chaine) Then
                    pasN = pasN + 1
                    e.Graphics.DrawImage(Image.FromFile("bmp\" & chaine & ".bmp"), New Rectangle(15 + pasN * larg, 1, larg, larg), 0, 0, 37, 37, GraphicsUnit.Pixel, transparence)
                ElseIf InStr(chaine, "B") = Len(chaine) Then
                    pasB = pasB + 1
                    e.Graphics.DrawImage(Image.FromFile("bmp\" & chaine & ".bmp"), New Rectangle(e.ClipRectangle.Width / 2 + pasB * larg, 1, larg, larg), 0, 0, 37, 37, GraphicsUnit.Pixel, transparence)
                End If
            End If
        Next

    End Sub

    Public Function epdCasesOccupees(fen As String) As String
        Dim tabColonne() As String, casesOccupees As String, tabLignes() As String, chaine As String, caractere As String
        Dim ligne As Integer, index As Integer, coups As String

        tabColonne = {"", "a", "b", "c", "d", "e", "f", "g", "h"}
        casesOccupees = "a1;b1;c1;d1;e1;f1;g1;h1;a2;b2;c2;d2;e2;f2;g2;h2;a7;b7;c7;d7;e7;f7;g7;h7;a8;b8;c8;d8;e8;f8;g8;h8;"
        coups = ""

        'pour chaque ligne
        tabLignes = Split(gauche(fen, fen.IndexOf(" ")), "/")
        For ligne = UBound(tabLignes) To 0 Step -1
            'on remplace les chiffres par des "-" qui indique "case vide"
            chaine = ""
            For index = 0 To Len(tabLignes(ligne)) - 1
                caractere = tabLignes(ligne).Substring(index, 1)
                If IsNumeric(caractere) Then
                    chaine = chaine & StrDup(CInt(caractere), "-")
                Else
                    chaine = chaine & caractere
                End If
                Application.DoEvents()
            Next
            tabLignes(ligne) = chaine

            'on met à jour les cases occupées
            For index = 0 To Len(tabLignes(ligne)) - 1
                If tabLignes(ligne).Substring(index, 1) = "-" Then
                    'on efface
                    casesOccupees = Replace(casesOccupees, tabColonne(index + 1) & Format(tabLignes.Length - ligne) & ";", "")
                Else
                    'on ajoute
                    chaine = tabColonne(index + 1) & Format(tabLignes.Length - ligne)
                    caractere = tabLignes(ligne).Substring(index, 1)
                    If caractere = "p" Or caractere = "P" Then
                        caractere = ""
                    End If
                    coups = coups & UCase(caractere) & chaine & "-" & chaine & " "
                    If InStr(casesOccupees, chaine & ";") = 0 Then
                        casesOccupees = casesOccupees & chaine & ";"
                    End If
                End If
                Application.DoEvents()
            Next
            Application.DoEvents()
        Next

        casesOccupees = trierChaine(casesOccupees, ";") & ";"

        Return casesOccupees & ":" & coupsFR(coups)
    End Function

    Public Function epdTotalBlanc(fen As String) As Integer
        Dim tabCaracteres() As Char, i As Integer, total As Integer

        'initialisation
        total = 0

        tabCaracteres = fen.ToCharArray
        For i = 0 To UBound(tabCaracteres)
            If tabCaracteres(i) = " " Then
                Exit For
            End If

            'pièces
            Select Case tabCaracteres(i)
                Case "R"
                    total = total + 5

                Case "N"
                    total = total + 3

                Case "B"
                    total = total + 3

                Case "Q"
                    total = total + 9

                Case "P"
                    total = total + 1

            End Select
        Next

        Return total

    End Function

    Public Function epdTotalNoir(fen As String) As Integer
        Dim tabCaracteres() As Char, i As Integer, total As Integer

        'initialisation
        total = 0

        tabCaracteres = fen.ToCharArray
        For i = 0 To UBound(tabCaracteres)
            If tabCaracteres(i) = " " Then
                Exit For
            End If

            'pièces
            Select Case tabCaracteres(i)
                Case "r"
                    total = total + 5

                Case "n"
                    total = total + 3

                Case "b"
                    total = total + 3

                Case "q"
                    total = total + 9

                Case "p"
                    total = total + 1

            End Select
        Next

        Return total

    End Function

    Public Function estimation(dureeMax As Integer, reprise As Integer, dureeMoy As Integer, transition As Single, message As String) As String
        Dim tabMax() As String, posRestantes As Integer, tabMin() As String, chaine As String

        chaine = message & " :"
        If mode = ".pgn" Then
            If Not tabPositions Is Nothing Then
                posRestantes = tabPositions.Length - reprise + 1
                tabMax = Split(secJHMS(posRestantes * (dureeMax + transition)), ";")
                If 0 < dureeMoy And dureeMoy < dureeMax Then
                    tabMin = Split(secJHMS(posRestantes * (dureeMoy + transition)), ";")
                    chaine = message & " : " & tabMin(0) & " d " & tabMin(1) & " h " & tabMin(2) & " m - " & tabMax(0) & " d " & tabMax(1) & " h " & tabMax(2) & " m"
                Else
                    chaine = message & " : " & tabMax(0) & " d " & tabMax(1) & " h " & tabMax(2) & " m " & tabMax(3) & " s"
                End If
            Else
                chaine = message & " : 0 d 0 h 0 m 0 s"
            End If
        ElseIf mode = ".epd" Then
            If Not tabEPD Is Nothing Then
                posRestantes = tabEPD.Length - reprise + 1
                tabMax = Split(secJHMS(posRestantes * (dureeMax + transition)), ";")
                If 0 < dureeMoy And dureeMoy < dureeMax Then
                    tabMin = Split(secJHMS(posRestantes * (dureeMoy + transition)), ";")
                    chaine = message & " : " & tabMin(0) & " d " & tabMin(1) & " h " & tabMin(2) & " m - " & tabMax(0) & " d " & tabMax(1) & " h " & tabMax(2) & " m"
                Else
                    chaine = message & " : " & tabMax(0) & " d " & tabMax(1) & " h " & tabMax(2) & " m " & tabMax(3) & " s"
                End If
            Else
                chaine = message & " : 0 j 0 h 0 m 0 s"
            End If
        End If

        Return chaine
    End Function

    Private Sub evenement1(sendingProcess As Object, donnees As DataReceivedEventArgs)
        If InStr(sortie1, donnees.Data) = 0 Then
            sortie1 = sortie1 & donnees.Data & vbCrLf
        End If
    End Sub

    Private Sub evenement2(sendingProcess As Object, donnees As DataReceivedEventArgs)
        If InStr(sortie2, donnees.Data) = 0 Then
            sortie2 = sortie2 & donnees.Data & vbCrLf
        End If
    End Sub

    Public Function extensionFichier(chemin As String) As String
        Dim fichier As New FileInfo(chemin)
        Return fichier.Extension
    End Function

    Public Function formaterCoups(mode As String, coups As String, Optional index As Integer = 0) As String
        Dim chaine As String, i As Integer, tabChaine() As String

        chaine = Replace(coups, "+", "")
        chaine = Replace(chaine, " mate", "")

        Select Case mode
            Case "arena", "texte"
                'indexation
                If index > 0 Then
                    chaine = Format(Int(index / 2) + 1) & ". " & chaine
                End If

            Case "pgn"
                'on supprime les commentaires
                chaine = Replace(Replace(chaine, "??", ""), "!!", "")
                'indexation
                If index > 0 Then
                    chaine = Format(Int(index / 2) + 1) & ". " & chaine
                End If

            Case "moteur"
                'on intercepte les roques
                'coups blanc
                If (index Mod 2) = 1 Then
                    If InStr(chaine, "0-0-0", CompareMethod.Text) > 0 Then
                        chaine = "e1c1"
                    ElseIf InStr(chaine, "0-0", CompareMethod.Text) > 0 Then
                        chaine = "e1g1"
                    End If
                Else
                    'coups noir
                    If InStr(chaine, "0-0-0", CompareMethod.Text) > 0 Then
                        chaine = "e8c8"
                    ElseIf InStr(chaine, "0-0", CompareMethod.Text) > 0 Then
                        chaine = "e8g8"
                    End If
                End If
                'on supprime les commentaires
                chaine = Replace(Replace(chaine, "??", ""), "!!", "")
                'on retient uniquement la postion de départ/arrivée
                chaine = Replace(Replace(chaine, "-", ""), "x", "")
                'on supprime la prise en passant
                chaine = Replace(chaine, "ep", "")
                'on supprime l'info du type de pièce
                If Len(chaine) = 5 And IsNumeric(droite(chaine, 1)) Then
                    chaine = droite(chaine, 4)
                ElseIf InStr(chaine, "=", CompareMethod.Text) > 0 Then
                    chaine = coupsEN(Replace(chaine, "=", "")) 'g7g8=C
                End If

            Case "doublon"
                'on supprime les commentaires
                chaine = Replace(Replace(chaine, "??", ""), "!!", "")
                'on supprime l'indexation
                tabChaine = Split(chaine, " ")
                chaine = ""
                For i = 0 To UBound(tabChaine)
                    If InStr(tabChaine(i), ".", CompareMethod.Text) = 0 Then
                        chaine = chaine & tabChaine(i) & " "
                    End If
                Next
                chaine = Trim(chaine)

            Case Else
                'on supprime que les commentaires
                chaine = Replace(Replace(chaine, "??", ""), "!!", "")

        End Select

        Return chaine
    End Function

    Public Function gauche(texte As String, longueur As Integer) As String
        If longueur > 0 Then
            Return VB.Left(texte, longueur)
        Else
            Return ""
        End If
    End Function

    Public Function maxProfMoteur(chaine As String) As Integer
        maxProfMoteur = 1
        If InStr(chaine, "asmfish", CompareMethod.Text) > 0 Then
            maxProfMoteur = 127
        ElseIf InStr(chaine, "brainfish", CompareMethod.Text) > 0 _
            Or InStr(chaine, "brainlearn", CompareMethod.Text) > 0 _
            Or InStr(chaine, "stockfish", CompareMethod.Text) > 0 _
            Or InStr(chaine, "cfish", CompareMethod.Text) > 0 _
            Or InStr(chaine, "sugar", CompareMethod.Text) > 0 _
            Or InStr(chaine, "eman", CompareMethod.Text) > 0 _
            Or InStr(chaine, "hypnos", CompareMethod.Text) > 0 _
            Or InStr(chaine, "judas", CompareMethod.Text) > 0 _
            Or InStr(chaine, "aurora", CompareMethod.Text) > 0 Then
            maxProfMoteur = 245
        ElseIf InStr(chaine, "houdini", CompareMethod.Text) > 0 Or InStr(chaine, "komodo", CompareMethod.Text) > 0 Then
            maxProfMoteur = 99
        End If

        Return maxProfMoteur
    End Function

    Public Function nomFichier(chemin As String) As String
        Return My.Computer.FileSystem.GetName(chemin)
    End Function

    Public Sub pgnEPD(fichier As String, Optional nettoyer As Boolean = True)
        Dim nom As String, commande As New Process()
        Dim dossierFichier As String, dossierTravail As String

        dossierFichier = fichier.Substring(0, fichier.LastIndexOf("\"))
        dossierTravail = Environment.CurrentDirectory
        nom = Replace(nomFichier(fichier), ".pgn", "")

        'si pgn-extract.exe ne se trouve dans le même dossier que le notre application
        If Not My.Computer.FileSystem.FileExists("pgn-extract.exe") Then
            'on cherche s'il se trouve dans le même dossier que le fichierPGN
            dossierTravail = dossierFichier
            If Not My.Computer.FileSystem.FileExists(dossierFichier & "\pgn-extract.exe") Then
                dossierTravail = "D:\JEUX\ARENA CHESS 3.5.1\Databases\PGN Extract GUI"
                If Not My.Computer.FileSystem.FileExists(dossierTravail & "\pgn-extract.exe") Then
                    dossierTravail = "E:\JEUX\ARENA CHESS 3.5.1\Databases\PGN Extract GUI"
                    If Not My.Computer.FileSystem.FileExists(dossierTravail & "\pgn-extract.exe") Then
                        'pgn-extract.exe est introuvable
                        MsgBox("Veuillez copier pgn-extract.exe et nettoyer_epd.exe dans :" & vbCrLf & dossierTravail, MsgBoxStyle.Critical)
                        dossierTravail = Environment.CurrentDirectory
                        If Not My.Computer.FileSystem.FileExists(dossierTravail & "\pgn-extract.exe") Then
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If

        'si le fichierPGN ne se trouve pas dans le dossier de travail
        If dossierFichier <> dossierTravail Then
            'on recopie temporairement le fichierPGN dans le dossierTravail
            My.Computer.FileSystem.CopyFile(fichier, dossierTravail & "\" & nom & ".pgn")
        End If

        'on écrit le chemin du fichierPGN dans le fichier .lst
        My.Computer.FileSystem.WriteAllText(dossierTravail & "\" & nom & ".lst", dossierTravail & "\" & nom & ".pgn", False, System.Text.Encoding.ASCII)

        commande.StartInfo.FileName = dossierTravail & "\pgn-extract.exe"
        commande.StartInfo.WorkingDirectory = dossierTravail
        commande.StartInfo.Arguments = " -Wepd -o" & nom & "_nettoyer.epd" & " -f" & nom & ".lst" & " -l" & nom & ".log"
        commande.StartInfo.UseShellExecute = False
        commande.Start()
        commande.WaitForExit()

        If nettoyer Then
            commande.StartInfo.FileName = dossierTravail & "\nettoyer_epd.exe"
            commande.StartInfo.WorkingDirectory = dossierTravail
            commande.StartInfo.Arguments = " " & nom & "_nettoyer.epd"
            commande.StartInfo.UseShellExecute = False
            commande.Start()
            commande.WaitForExit()
        End If

        My.Computer.FileSystem.RenameFile(dossierTravail & "\" & nom & "_nettoyer.epd", nom & ".epd")
        My.Computer.FileSystem.DeleteFile(dossierTravail & "\" & nom & ".lst")

        'si le dossierTravail ne correspond pas au dossier du fichierPGN
        If dossierFichier <> dossierTravail Then
            'on déplace le fichier .epd
            My.Computer.FileSystem.DeleteFile(dossierTravail & "\" & nom & ".pgn")
            My.Computer.FileSystem.MoveFile(dossierTravail & "\" & nom & ".epd", dossierFichier & "\" & nom & ".epd")
            My.Computer.FileSystem.MoveFile(dossierTravail & "\" & nom & ".log", dossierFichier & "\" & nom & ".log")
        End If

    End Sub

    Public Function secJHMS(ByVal secondes As Long) As String
        Dim restant As Integer, valeur As Integer, chaine As String

        restant = secondes

        valeur = Fix(restant / 60 / 60 / 24) 'jours
        chaine = valeur
        restant = restant - valeur * 60 * 60 * 24

        valeur = Fix(restant / 60 / 60) 'heures
        chaine = chaine & ";" & valeur
        restant = restant - valeur * 60 * 60

        valeur = Fix(restant / 60) 'minutes
        chaine = chaine & ";" & valeur
        restant = restant - valeur * 60

        valeur = Fix(restant) 'secondes
        chaine = chaine & ";" & valeur

        'ex : secondes = 34849 => secJHMS = "0;9;40;49" ("Jours;Heures;Minutes;Secondes")

        Return chaine
    End Function

    Public Function trierChaine(serie As String, separateur As String, Optional ordre As Boolean = True) As String
        Dim tabChaine() As String

        tabChaine = Split(serie, separateur)
        If tabChaine(UBound(tabChaine)) = "" Then
            ReDim Preserve tabChaine(UBound(tabChaine) - 1)
        End If

        Array.Sort(tabChaine)
        If Not ordre Then
            Array.Reverse(tabChaine)
        End If

        Return String.Join(separateur, tabChaine)
    End Function

End Module
