using System.IO;
using System.Threading;

namespace IcuAVE
{
    public class Test
    {
        public void RunTest()
        {
            string fileName = Path.GetTempFileName();

            if (File.Exists(fileName))
                File.Delete(fileName);

            // Copy the file to the local system. The error only occurs when using a FileStream
            // to read the data. It doesn't happen when using a ManifestResourceStream.
            using (var source = GetType().Assembly.GetManifestResourceStream("IcuAVE.fail-scenario-1.txt"))
            using (Stream destination = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                source.CopyTo(destination);
            }


            int numThreads = 8;

            Thread[] threads = new Thread[numThreads];


            CountdownEvent startingGun = new CountdownEvent(1);
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => RunAVETest(startingGun, fileName));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start();
            }

            startingGun.Signal();
            foreach (var t in threads)
            {
                t.Join();
            }
        }

        private void RunAVETest(CountdownEvent latch, string fileName)
        {
            latch.Wait();

            int repeatCount = 100;

            for (int i = 0; i < repeatCount; i++)
            {
                var locale = new global::Icu.Locale("en_US");

                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read,FileShare.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string text;
                        while ((text = reader.ReadLine()) != null)
                        {
                            global::Icu.BreakIterator.GetWordBoundaries(locale, text, true);
                        }
                    }
                }
            }
        }

    }
}
