using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjekatWPF
{
   public class DB
    {
        SqlConnection conn;
        SqlCommand cmd; 
        Organizacija o = new Organizacija();
        private Racun ra = new Racun();
        public DB()
        {
            conn = new SqlConnection("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=HumanitarnaOrganizacija;Integrated Security=True");
            cmd = conn.CreateCommand();
        }
        public List<Organizacija> GetOrganizacija()
        {
            List<Organizacija> organizacija = new List<Organizacija>(); //prazna
            try
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM Organizacija";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    organizacija.Add(new Organizacija()
                    {
                        Ime = reader["ime"].ToString(),
                        Donacija = float.Parse(reader["donacije"].ToString()),

                    });
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return organizacija;
        }
        public List<Ponuda> GetPonude()
        {
            List<Ponuda> listaPonuda
                = new List<Ponuda>(); //prazna
            try
            {
                conn.Open();
                cmd.CommandText = "SELECT * FROM Odabir";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listaPonuda.Add(new Ponuda()
                    {
                        ImeOrg = reader["ImeOrg"].ToString(),
                        PotrebnaKol = float.Parse(reader["PotrebnaKol"].ToString()),
                        Tekst = reader["Tekst"].ToString()
                    });
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return listaPonuda;
        }

        public void IzmeniDonaciju(string imeOrg, float novaDonacija)
        {
            try
            {
                o.Donacija = novaDonacija;
                float staraDonacija=0;
                conn.Open();
                cmd.CommandText = $"SELECT [donacije] FROM Organizacija WHERE [ime]='{imeOrg}'";
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    try
                    {

                    staraDonacija = float.Parse(r["donacije"].ToString());
                    }
                    catch { staraDonacija = 0; }
                }
                r.Close();
                cmd.CommandText = $"UPDATE Organizacija SET [donacije]='{ novaDonacija + staraDonacija }' WHERE [ime]='{imeOrg}'";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesno doniranje");
            }
            catch(SqlException ex) { MessageBox.Show(ex.Message); }
            finally { if (conn != null) { conn.Close(); } }
        }
        public void IzmeniPonudu(string imeOrg, float novaDonacija)
        {
            try
            {
                o.Donacija = novaDonacija;
                float staraDonacija = 0;
                conn.Open();
                cmd.CommandText = $"SELECT [PotrebnaKol] FROM Odabir WHERE [ImeOrg]='{imeOrg}'";
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    try
                    {

                        staraDonacija = float.Parse(r["PotrebnaKol"].ToString());
                    }
                    catch { staraDonacija = 0; }
                }
                r.Close();
                if (staraDonacija - novaDonacija <= 0)
                {
                    cmd.CommandText = $"DELETE FROM Odabir WHERE  [ImeOrg]='{imeOrg}'";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = $"UPDATE Odabir SET [PotrebnaKol]='{staraDonacija - novaDonacija}' WHERE [ImeOrg]='{imeOrg}'";
                    cmd.ExecuteNonQuery();
                }              
                //MessageBox.Show("Uspesno doniranje");
            }
            catch (SqlException ex) { MessageBox.Show(ex.Message); }
            finally { if (conn != null) { conn.Close(); } }
        }


        public void Naplati(string BrRacuna, float Naplata, string nazivOrganizacije)
        {
            try
            {
                float staraDonacija = 0;
                float stanje = 0;
                conn.Open();

               
                cmd.CommandText = $"SELECT [KolicinaNovca] FROM Kartica WHERE [BrRacuna]='{BrRacuna}'";
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    try
                    {
                        staraDonacija = float.Parse(r["KolicinaNovca"].ToString());
                    }
                    catch { staraDonacija = 0; }
                }
                r.Close();

                
                if (staraDonacija < Naplata)
                {
                    MessageBox.Show("Nemate dovoljno sredstava na kartici.");
                    return;
                }

               
                float novoStanjeKartice = staraDonacija - Naplata;
                cmd.CommandText = $"UPDATE Kartica SET [KolicinaNovca]='{novoStanjeKartice}' WHERE [BrRacuna]='{BrRacuna}'";
                cmd.ExecuteNonQuery();
             
                cmd.CommandText = $"SELECT [PotrebnaKol] FROM Odabir WHERE [ImeOrg]='{nazivOrganizacije}'";
                r = cmd.ExecuteReader();
                while (r.Read())
                {
                    try
                    {
                        stanje = float.Parse(r["PotrebnaKol"].ToString());
                    }
                    catch { stanje = 0; }
                }
                r.Close();

                float novoStanje = stanje - Naplata;

                if(novoStanje > 0)
                {
                    cmd.CommandText = $"UPDATE Odabir SET [PotrebnaKol]='{novoStanje}' WHERE [ImeOrg]='{nazivOrganizacije}'";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = $"DELETE FROM Odabir WHERE [ImeOrg]='{nazivOrganizacije}'";
                    cmd.ExecuteNonQuery();
                }
                
                
                cmd.CommandText = $"SELECT [donacije] FROM Organizacija WHERE [ime]='{nazivOrganizacije}'";
                r = cmd.ExecuteReader();
                while (r.Read())
                {
                    try
                    {
                        staraDonacija = float.Parse(r["donacije"].ToString());
                    }
                    catch { staraDonacija = 0; }
                }
                r.Close();

               
                float novaDonacija = staraDonacija + Naplata;

                cmd.CommandText = $"UPDATE Organizacija SET [donacije]='{novaDonacija}' WHERE [ime]='{nazivOrganizacije}'";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Uspesno doniranje");
                ra.FirstMessageBoxText = $"Dodato {Naplata:C} donacija za organizaciju '{nazivOrganizacije}'.\nTrenutno stanje donacija: {novaDonacija:C}";
                ra.SecondMessageBoxText = $"Hvala Vam sto pomazete nasoj organizaciji sa iznosom od: {Naplata:C}";
                ra.Show();
                ra.ShowMessageBox();

                
            }
            catch (SqlException ex) { MessageBox.Show(ex.Message); }
            finally { if (conn != null) { conn.Close(); } }
        }
        public Kartica UcitajKarticu(string BrRacuna) 
        {
            Kartica kartica = new Kartica();
            try
            {
                conn.Open();
                cmd.CommandText = $"SELECT KolicinaNovca FROM Kartica WHERE BrRacuna='{BrRacuna}'";
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    kartica.KolicinaNovca = float.Parse(r["KolicinaNovca"].ToString());
                    kartica.BrRacuna = BrRacuna;
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return kartica;
        }
        public Korisnik Logovanje(string Ime)
        {
            Korisnik korisnik = new Korisnik();
            try
            {
                conn.Open();
                cmd.CommandText = $"SELECT KorisnickoIme,Sifra,Uloga FROM Korisnikk WHERE KorisnickoIme='{Ime}'";
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    korisnik.KorisnickoIme = r["KorisnickoIme"].ToString();
                    korisnik.Sifra = r["Sifra"].ToString();
                    korisnik.Uloga = r["Uloga"].ToString();
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return korisnik;
        }
        public void Registracija(Korisnik korisnik)
        {
            try
            {
                conn.Open();
                cmd.CommandText = $"INSERT INTO [dbo].[Korisnikk] ([KorisnickoIme],[Sifra],[Uloga]) VALUES('{korisnik.KorisnickoIme}','{korisnik.Sifra}','User')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesna registacija");
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void UnosOrganizacije(Organizacija o)
        {
            try
            {
                conn.Open();
                cmd.CommandText = $"INSERT INTO [dbo].[Organizacija] ([ime],[donacije]) VALUES('{o.Ime}','{o.Donacija}')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesan upis");
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public void IzmeniOrganizaciju(string StaroIme,Organizacija o)
        {
            try
            {
                conn.Open();
                cmd.CommandText = $"UPDATE [dbo].[Organizacija] SET [ime]='{o.Ime}',[donacije]='{o.Donacija}' WHERE [ime]='{StaroIme}'";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Uspesna izmena");
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

    }
}
