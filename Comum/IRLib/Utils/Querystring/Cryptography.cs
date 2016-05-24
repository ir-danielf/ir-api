using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Utils.Extensions;

namespace Utils.Cryptography
{
    //types of symmetric encyption
    public enum EncryptionTypes
    {
        DES,
        RC2,
        Rijndael,
        TripleDES
    }

    //direction fo the transform
    public enum TransformDirection
    {
        Encrypt,
        Decrypt
    }

    /// <summary>
    /// basic Encryption/decryption functionaility
    /// </summary>
    public class Encryption
    {
        #region enums, constants & fields
        private const string DEFAULT_PASSWORD = "abcd!@#";
        private const EncryptionTypes DEFAULT_ALGORITHM = EncryptionTypes.Rijndael;

        private byte[] _key; // cryptographic secret key
        private byte[] _IV; //initialization vector
        private byte[] _saltByteArray = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }; //default salt

        private EncryptionTypes _encryptionType = DEFAULT_ALGORITHM;
        private string _password = DEFAULT_PASSWORD;
        private bool _calculateNewKeyAndIV = true;
        #endregion

        #region Constructors
        public Encryption()
        {
        }

        public Encryption(EncryptionTypes type)
        {
            _encryptionType = type;
        }
        #endregion

        #region Props

        /// <summary>
        /// type of encryption / decryption used
        /// </summary>
        public EncryptionTypes EncryptionType
        {
            get { return _encryptionType; }
            set
            {
                if (_encryptionType != value)
                {
                    _encryptionType = value;
                    _calculateNewKeyAndIV = true;
                }
            }
        }

        /// <summary>
        ///	Passsword Key Property.
        /// The password key used when encrypting / decrypting
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    _calculateNewKeyAndIV = true;
                }
            }
        }

        /// <summary>
        /// The Salt that is used. This can only be set
        /// </summary>
        public byte[] Salt
        {
            set
            {
                if (_saltByteArray != value)
                {
                    _saltByteArray = value;
                    _calculateNewKeyAndIV = true;
                }
            }
        }
        #endregion

        #region Encryption

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="inputData">byte array to encrypt</param>
        /// <returns>an encrypted byte array</returns>
        public byte[] Encrypt(byte[] inputData)
        {
            return Transform(inputData, TransformDirection.Encrypt);
        }

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="inputText">text to encrypt</param>
        /// <returns>an encrypted string</returns>
        public string Encrypt(string inputText)
        {
            //convert back to a string
            return Encrypt(inputText.ToByteArrayUTF8()).ToBase64String();
        }

        /// <summary>
        /// Static encrypt method
        /// </summary>
        public static string EncryptText(string inputText)
        {
            return EncryptText(inputText, DEFAULT_ALGORITHM);
        }

        /// <summary>
        /// Static encrypt method
        /// </summary>
        public static string EncryptText(string inputText, EncryptionTypes type)
        {
            return new Encryption(type).Encrypt(inputText);
        }

        #endregion

        #region Decryption

        /// <summary>
        ///		decrypts a string
        /// </summary>
        /// <param name="inputText">string to decrypt</param>
        /// <returns>a decrypted string</returns>
        public string Decrypt(string inputText)
        {
            //convert back to a string
            return Decrypt(inputText.ToByteArrayBase64()).ToUTF8String();
        }

        /// <summary>
        /// decrypts a byte array
        /// </summary>
        /// <param name="inputData">byte array to decrypt</param>
        /// <returns>a decrypted byte array</returns>
        public byte[] Decrypt(byte[] inputData)
        {
            return Transform(inputData, TransformDirection.Decrypt);
        }

        /// <summary>
        /// Static Decrypt method
        /// </summary>
        public static string DecryptText(string inputText)
        {
            return DecryptText(inputText, DEFAULT_ALGORITHM);
        }

        /// <summary>
        /// Static Decrypt method
        /// </summary>
        public static string DecryptText(string inputText, EncryptionTypes type)
        {
            return new Encryption(type).Decrypt(inputText);
        }

        #endregion

        #region Symmetric Engine

        /// <summary>
        ///		performs the actual enc/dec.
        /// </summary>
        /// <param name="inputBytes">input byte array</param>
        /// <param name="Encrpyt">wheather or not to perform enc/dec</param>
        /// <returns>byte array output</returns>
        private byte[] Transform(byte[] inputBytes, TransformDirection direction)
        {
            //get the correct transform
            ICryptoTransform transform = GetEncryptionTransform(direction);

            //memory stream for output
            MemoryStream memStream = new MemoryStream();

            try
            {
                //setup the cryption - output written to memstream
                CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

                //write data to cryption engine
                cryptStream.Write(inputBytes, 0, inputBytes.Length);

                //we are finished
                cryptStream.FlushFinalBlock();

                //get result
                byte[] output = memStream.ToArray();

                //finished with engine, so close the stream
                cryptStream.Close();

                return output;
            }
            catch (Exception e)
            {
                //throw an error
                throw new Exception("Error in symmetric engine. Error : " + e.Message, e);
            }
        }

        /// <summary>
        ///		returns the symmetric engine and creates the encyptor/decryptor
        /// </summary>
        /// <param name="encrypt">whether to return a encrpytor or decryptor</param>
        /// <returns>ICryptoTransform</returns>
        private ICryptoTransform GetEncryptionTransform(TransformDirection direction)
        {
            if (_calculateNewKeyAndIV)
                CalculateNewKeyAndIV();
            if (direction == TransformDirection.Encrypt)
                return GetEncryptionAlgorithm().CreateEncryptor(_key, _IV);
            else
                return GetEncryptionAlgorithm().CreateDecryptor(_key, _IV);
        }
        /// <summary>
        ///		returns the specific symmetric algorithm acc. to the cryptotype
        /// </summary>
        /// <returns>SymmetricAlgorithm</returns>
        private SymmetricAlgorithm GetEncryptionAlgorithm()
        {
            switch (_encryptionType)
            {
                case EncryptionTypes.DES:
                    return DES.Create();
                case EncryptionTypes.RC2:
                    return RC2.Create();
                case EncryptionTypes.Rijndael:
                    return Rijndael.Create();
                default:
                    return TripleDES.Create(); //default
            }
        }

        /// <summary>
        ///		calculates the key and IV acc. to the symmetric method from the password
        ///		key and IV size dependant on symmetric method
        /// </summary>
        private void CalculateNewKeyAndIV()
        {
            //use salt so that key cannot be found with dictionary attack
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_password, _saltByteArray);
            SymmetricAlgorithm algo = GetEncryptionAlgorithm();
            _key = pdb.GetBytes(algo.KeySize / 8);
            _IV = pdb.GetBytes(algo.BlockSize / 8);
        }

        #endregion
    }
}