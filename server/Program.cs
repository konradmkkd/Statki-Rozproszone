using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class MultiThreadedEchoServer
{
    static int counter = 0;
    static string[] shipsArray = new string[10];
   //dodaje się poprawnie tylko gracz1 pierwszy jest na indeksie 2 a gracz2 na indeksie 3
 
    private static void ProcessClientRequests(object argument)
    {
     

        TcpClient client = (TcpClient)argument;
        try
        {
    
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            string s = String.Empty;


        
            if (counter == 2)
            {
                writer.WriteLine("Kolej klienta 2");
                writer.Flush();
                String client_string = reader.ReadLine();
            }
            if(counter == 3)
            {
                writer.WriteLine("Kolej klienta 1");
                writer.Flush();
                String client_string = reader.ReadLine();
            }

            while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
            {
               
                shipsArray[counter] = s;

                Console.WriteLine("Client ->" + s);
                writer.WriteLine("From server -> " + s);
                writer.Flush();
            }
            reader.Close();
            writer.Close();
            client.Close();
            Console.WriteLine("Closing client connection!");

        
        }
        catch (IOException)
        {
            Console.WriteLine("Problem with client communication. Exiting thread.");
        }
        finally
        {
            if (client != null)
            {
                client.Close();
            }


        }
      
    }
    public static void Main()
	{
		TcpListener listener = null;
		try
		{
			listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
			listener.Start();

		
			while (true)
			{
				counter++;
                Console.WriteLine("Oczkiwanie na graczy..");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Gracz " + counter + " dołączył do gry");
                if (counter == 1)
                {
                    Thread t1 = new Thread(ProcessClientRequests);
                    t1.Start(client);
                    
                }

                if (counter == 2)
                {
                    Thread t2 = new Thread(ProcessClientRequests);
                    t2.Start(client);
                    Console.WriteLine("Można rozpocząć grę");
                    
                }
                for (int i = 0; i < shipsArray.Length; i++)
                {
                    if (shipsArray[i] != null)
                        Console.WriteLine(shipsArray[i] + " indeks: " + i);
                }
               
               

			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			if (listener != null)
			{
				listener.Stop();
			}
		}
       
    } // end Main()
} // end class definition