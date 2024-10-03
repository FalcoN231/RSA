using System;
using System.Collections.Generic;

namespace App
{
      public class GFG
      {
            //Таблица простых чисел
            private static HashSet<int> prime = new HashSet<int>();

            //Публичный ключ
            private static int? public_key = null;

            //Приватный ключ
            private static int? private_key = null;

            //Модуль системы RSA
            private static int? n = null;


            private static Random random = new Random();

            public static void Main()
            {
                  //Преинициализация
                  PrimeFiller();
                  SetKeys();

                  string message = "Test Message";

                  //Шифрование текста публичным ключем
                  List<int> coded = Encoder(message);

                  Console.WriteLine("Initial message:");
                  Console.WriteLine(message);
                  Console.WriteLine("\n\nThe encoded message (encrypted by public key)\n");
                  Console.WriteLine(string.Join("", coded));
                  Console.WriteLine("\n\nThe decoded message (decrypted by public key)\n");
                  Console.WriteLine(Decoder(coded));
            }

            // Генератор таблицы простых чисел
            public static void PrimeFiller(int border = 250)
            {
                  bool[] sieve = new bool[border];

                  sieve[0] = true;
                  sieve[1] = true;

                  for (int i = 2; i < border; i++)
                  {
                        for (int j = i * 2; j < border; j += i)
                        {
                              sieve[j] = true;
                        }
                  }

                  for (int i = 0; i < border; i++)
                  {
                        if (!sieve[i])
                        {
                              prime.Add(i);
                        }
                  }
            }

            //Из таблицы простхы чисел забираем 1 случайное
            public static int PickRandomPrime()
            {
                  int k = random.Next(0, prime.Count - 1);
                  var enumerator = prime.GetEnumerator();
                  for (int i = 0; i <= k; i++)
                  {
                        enumerator.MoveNext();
                  }

                  int ret = enumerator.Current;
                  prime.Remove(ret);
                  return ret;
            }

            public static void SetKeys()
            {
                  int prime1 = PickRandomPrime();
                  int prime2 = PickRandomPrime();

                  n = prime1 * prime2;
                  int fi = (prime1 - 1) * (prime2 - 1);

                  int e = 2;
                  while (true)
                  {
                        if (GCD(e, fi) == 1)
                        {
                              break;
                        }
                        e += 1;
                  }

                  public_key = e;

                  int d = 2;
                  while (true)
                  {
                        if ((d * e) % fi == 1)
                        {
                              break;
                        }
                        d += 1;
                  }

                  private_key = d;
            }
            public static int Encrypt(int message)
            {
                  int e = public_key.Value;
                  int encrypted_text = 1;
                  while (e > 0)
                  {
                        encrypted_text *= message;
                        encrypted_text %= n.Value;
                        e -= 1;
                  }
                  return encrypted_text;
            }

            public static int Decrypt(int encrypted_text)
            {
                  int d = private_key.Value;
                  int decrypted = 1;
                  while (d > 0)
                  {
                        decrypted *= encrypted_text;
                        decrypted %= n.Value;
                        d -= 1;
                  }
                  return decrypted;
            }

            //Наибольший общий делитель
            public static int GCD(int a, int b)
            {
                  if (b == 0)
                  {
                        return a;
                  }
                  return GCD(b, a % b);
            }

            public static List<int> Encoder(string message)
            {
                  List<int> encoded = new List<int>();
                  foreach (char letter in message)
                  {
                        encoded.Add(Encrypt((int)letter));
                  }
                  return encoded;
            }

            public static string Decoder(List<int> encoded)
            {
                  string s = "";
                  foreach (int num in encoded)
                  {
                        s += (char)Decrypt(num);
                  }
                  return s;
            }


      }
}