using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dag_25
{
    class Program
    {
        static void Main(string[] args)
        {
            using var file = new FileStream("Input.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var publicKey1 = long.Parse(reader.ReadLine());
            var publicKey2 = long.Parse(reader.ReadLine());
            // test values:
            //long publicKey1 = 5764801;
            //long publicKey2 = 17807724;
            var subjectNumber = 7;
            var result = new Handshake(subjectNumber, publicKey1);
            Console.WriteLine($"Loopsize = {result.LoopSize}");
            var oplossing1 = new Handshake(publicKey2, result.LoopSize, true);
            Console.WriteLine($"EncryptionKey = {oplossing1.EncryptionKey}");
        }
        
    }
    public class Handshake
    {
        public long SubjectNumber { get;}
        public long PublicKey { get; }
        public long LoopSize { get; }
        public long Divider { get; }
        public long EncryptionKey { get; }
        public Handshake(long subjectNumber, long value, bool GetEncryptionKey = false)
        {
            if (GetEncryptionKey)
            {
                Console.WriteLine("Calculating Encryption Key");
                Divider = 20201227;
                SubjectNumber = subjectNumber;
                LoopSize = value;
                PublicKey = 0;
                EncryptionKey = Transform(LoopSize);
                //Console.WriteLine($"EncryptionKey = {EncryptionKey}");
            }
            else
            {
                Console.WriteLine("Calculating Loopsize");
                Divider = 20201227;
                SubjectNumber = subjectNumber;
                PublicKey = value;
                LoopSize = GetLoopSize(PublicKey);
                //Console.WriteLine($"LoopSize = {LoopSize}");
                EncryptionKey = 0;
            } 
        }
        public long GetLoopSize(long key)
        {
            long rounds = 1;
            long nextSubjectNumber = SubjectNumber;
            nextSubjectNumber = nextSubjectNumber % Divider;
            while (nextSubjectNumber!= key)
            {
                nextSubjectNumber *= SubjectNumber;
                nextSubjectNumber = nextSubjectNumber % Divider;
                rounds++;
                //if (rounds % 10000 == 0) Console.WriteLine($"Calculating Round: {rounds}");
            }
            return rounds;
        }
        public long Transform(long loopSize)
        {
            long nextSubjectNumber = SubjectNumber;
            nextSubjectNumber = nextSubjectNumber % Divider;
            for (var i = 1; i< loopSize; i++)
            {
                nextSubjectNumber *= SubjectNumber;
                nextSubjectNumber = nextSubjectNumber % Divider; 
            }
            return nextSubjectNumber;
        }
    }
}
