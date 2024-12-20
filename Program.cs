namespace BoostProjeGunlukKosuMesafesiOlcer
{
    internal class Program
    {
        //Toplam mesafenin hesaplandığı metot. Listeden adım sayısını alıcak, adım mesafesini alıcak ve toplam süreyi alıcak parametre olarak.
        static double CalculateTotalDistance(List<int> stepCounts, double stepLength, int totalMinutes)
        {
            double totalDistance = 0;
            int remainingTimeForCalculation = totalMinutes;
            int sectionMinutes = 0;
            //Listedeki her adım ve her dakika için hesabı yapmak için foreach kullandım.
            foreach (int stepRate in stepCounts)
            {
                if (remainingTimeForCalculation > 0)
                {
                    sectionMinutes = remainingTimeForCalculation;
                }
                totalDistance += sectionMinutes * stepRate * stepLength / 100; // Metre cinsinden uzaklığı hesaplıyoruz.
                remainingTimeForCalculation -= sectionMinutes;
            }
            return totalDistance;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Daily Running Distance Measurement Program!");

            //Programı döngüye sokan kısım. True olduğu sürece program çalışıcak.
            bool programRunning = true;
            while (programRunning)
            {
                try//Bütün programı try catch içerisine yazdım ki hataları detaylı bir şekilde ayıklayabileyim.
                {
                    //Kullanıcıdan adım mesafesinin istendiği kısım.
                    double stepLength;
                    //Burda döngüye sokuyoruz ki yanlış veya geçersiz bir değer girilse bile programdan çıkılmasın.
                    do
                    {
                        Console.Write("Please enter your average step length (in cm, max 150 cm): ");
                        //Ortalama adım mesafesinin maximum 150 olduğunu araştırdım o yüzden böyle bir limit koydum.
                        if (!double.TryParse(Console.ReadLine(), out stepLength) || stepLength <= 0 || stepLength > 150)
                        {
                            Console.WriteLine("Invalid input. Step length must be a positive number and cannot exceed 150 cm.");
                        }
                    } while (stepLength <= 0 || stepLength > 150);//Ascii değerleri saolsun, string girilse bile bu döngüden çıkmicak ve kullanıcıya hata vericek.

                    //Kullanıcıdan ne kadar süre koştuğunun bilgilerini isteyen kısım.
                    Console.Write("Please enter your running time in hours: ");
                    if (!int.TryParse(Console.ReadLine(), out int hours) || hours < 0)
                    {
                        throw new ArgumentException("Hour value must be a positive number and exclusively a number.");
                    }

                    Console.Write("Please enter your running time in minutes: ");
                    if (!int.TryParse(Console.ReadLine(), out int minutes) || minutes < 0 || minutes >= 60)
                    {
                        throw new ArgumentException("Minute value must be between 0 and 59.");
                    }

                    //Koşulan toplam dakikayı hesaplıyoruz burda daha kolay hesap tutmak için.
                    int totalMinutes = (hours * 60) + minutes;
                    if (totalMinutes <= 0)
                    {
                        throw new ArgumentException("Total running time must be positive.");
                    }

                    //Ek soru.
                    //Burda liste ile tutmaya karar verdim çünkü diziye kıyasla resize derdim olmaz ve kullanıcı örnek veriyorum:
                    //80dakika koşsa ve her dakikasında farklı adım sayısı girmek istese girebilir.
                    List<int> stepCounts = new List<int>();
                    //Toplam dakikasından kalan dakikasını hesaplıyoruz.
                    int remainingMinutes = totalMinutes;

                    Console.WriteLine("If you are running at different speeds, please enter the step count per minute for each section.");
                    //Kullanıcının koştuğu süre toplam dakikaya çevirmiştim yukarda burda kalan dakika üzerinden hesap yapılıcak.
                    while (remainingMinutes > 0)
                    {
                        //kullanıcının kaç dakika boyunca kaçar adım attığını soran kısım.
                        Console.Write($"Section minutes (Remaining: {remainingMinutes} minutes): ");
                        if (!int.TryParse(Console.ReadLine(), out int sectionMinutes) || sectionMinutes <= 0 || sectionMinutes > remainingMinutes)
                        {
                            Console.WriteLine("Invalid value. Please enter a positive number that is not greater than the remaining minutes.");
                            continue;
                        }
                        Console.Write("Step count per minute in this section: ");
                        if (!int.TryParse(Console.ReadLine(), out int stepsPerMinute) || stepsPerMinute <= 0)
                        {
                            Console.WriteLine("Step count must be a positive integer.");
                            continue;
                        }
                        //Girdiği adım sayısını listeye ekliyoruz.
                        stepCounts.Add(stepsPerMinute);
                        remainingMinutes -= sectionMinutes;
                    }

                    //Toplam mesafeyi hesaplayan metodu çağırıyorum burda bütün bilgileri aldıktan sonra.
                    double totalDistance = CalculateTotalDistance(stepCounts, stepLength, totalMinutes);

                    Console.WriteLine($"Your total running distance is: {totalDistance:F2} meters.");

                    //Kullanıcıdan çıkış yapmak isteyip istemediği soruluyor.
                    Console.Write("Would you like to exit the program? (Yes/No): ");
                    string exitResponse = Console.ReadLine()?.Trim().ToLower();

                    //yes veya no değeri girilmediği sürece kullanıcıya hatalı giriş yaptınız uyarısı veren kısım.
                    while (exitResponse != "yes" && exitResponse != "no")
                    {
                        Console.WriteLine("Invalid input. Please enter 'Yes' to continue or 'No' to exit.");
                        Console.Write("Would you like to exit the program? (Yes/No): ");
                        exitResponse = Console.ReadLine()?.Trim().ToLower();
                    }
                    //yes değeri girilirse programdan çıkış yapıcak. no girerse başa döndürücek.
                    if (exitResponse == "yes")
                    {
                        programRunning = false;
                    }
                    else
                    {
                        //Eğer programdan çıkış yapılması istenirse. 
                        Console.WriteLine("Program is starting again...\n");
                    }
                }
                //Format hatası veya diğer hataları yakaladığı yer.
                catch (FormatException ex)
                {
                    Console.WriteLine($"Input format error: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Please try again.");
                }
            }
            Console.WriteLine("\nYou have exited the program. Goodbye!");
        }
    }
}
