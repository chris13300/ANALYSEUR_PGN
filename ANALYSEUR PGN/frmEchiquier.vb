Public Class frmEchiquier

    Private Sub frmEchiquier_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        fermetureDemande = True
    End Sub

    Private Sub frmEchiquier_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        Me.Text = tabEPD(pos)
        Me.Location = emplacement
        echiquier(tabEPD(pos), e, Me.Width, Me.Height, coup1, coup2)
    End Sub

End Class
