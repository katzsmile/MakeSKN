using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;

namespace makeskn
{
    class Program
    {
        static int errors = 0;

        static void EmitError(string e)
        {
            Console.WriteLine("MakeSKN: Error: {0}", e);
            ++errors;
        }

        static void buildfile(string w3xfile, string fPath)
        {
            if (File.Exists(w3xfile))
            {
                try
                {
                    string Skeleton = "";
                    string Container = "";
                    ArrayList OBBoxes = new ArrayList();
                    ArrayList Meshes = new ArrayList();
                    ArrayList TexturesList = new ArrayList();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(w3xfile);
                    string nXML = w3xfile;
                    XmlNodeList AssetDeclarations = xDoc.GetElementsByTagName("AssetDeclaration");
                    XmlNodeList W3DContainers = xDoc.GetElementsByTagName("W3DContainer");
                    XmlElement newIncludes = xDoc.CreateElement("Includes", "uri:ea.com:eala:asset");
                    if (W3DContainers.Count != 0)
                    {
                        Console.Write("\nProcessing W3X Container: " + w3xfile);
                        foreach (XmlNode AssetDeclaration in AssetDeclarations)
                        {
                            foreach (XmlNode W3DContainer in W3DContainers)
                            {
                                XmlNamedNodeMap mapAttributes = W3DContainer.Attributes;
                                foreach (XmlNode xnodAttribute in mapAttributes)
                                {
                                    if (xnodAttribute.Name == "Hierarchy")
                                    {
                                        Skeleton = xnodAttribute.Value;
                                        Console.Write(".");
                                    }
                                    if (xnodAttribute.Name == "id")
                                    {
                                        Container = xnodAttribute.Value;
                                        Console.Write(".");
                                    }
                                }

                                XmlNodeList CollisionBoxs = xDoc.GetElementsByTagName("CollisionBox");
                                foreach (XmlNode CollisionBox in CollisionBoxs)
                                {
                                    string strValue = (string)CollisionBox.InnerText;
                                    OBBoxes.Add(strValue);
                                    Console.Write(".");
                                }
                                XmlNodeList Meshs = xDoc.GetElementsByTagName("Mesh");
                                foreach (XmlNode SingleMesh in Meshs)
                                {
                                    string strValue = (string)SingleMesh.InnerText;
                                    Meshes.Add(strValue);
                                    Console.Write(".");
                                }
                                if (Skeleton.Length != 0)
                                {
                                    XmlElement Include = xDoc.CreateElement("Include", "uri:ea.com:eala:asset");
                                    Include.SetAttribute("type", "all");
                                    Skeleton = Skeleton.ToLower();
                                    Include.SetAttribute("source", "ART:" + Skeleton + ".w3x");
                                    newIncludes.AppendChild(Include);
                                    xDoc.DocumentElement.InsertBefore(newIncludes, W3DContainer);
                                    xDoc.Save(nXML);
                                    Console.Write(".");
                                }
                                if (OBBoxes.Count != 0)
                                {
                                    foreach (string OBBox in OBBoxes)
                                    {
                                        string OBBoxFile = Path.Combine(fPath, (OBBox + ".w3x"));
                                        Console.Write(".");
                                        XmlTextReader reader = new XmlTextReader(OBBoxFile);
                                        while (reader.Read())
                                        {
                                            reader.ReadToFollowing("W3DCollisionBox");
                                            string strInner = reader.ReadOuterXml();
                                            if (strInner.Length != 0)
                                            {
                                                XmlTextReader xmlReader = new XmlTextReader(new StringReader(strInner));
                                                XmlNode OBBoxNode = xDoc.ReadNode(xmlReader);
                                                xDoc.DocumentElement.InsertBefore(OBBoxNode, W3DContainer);
                                                xDoc.Save(nXML);
                                                Console.Write(".");
                                            }
                                        }
                                        reader.Close();
                                    }
                                    foreach (string OBBox in OBBoxes)
                                    {
                                        string OBBoxFile = Path.Combine(fPath, (OBBox + ".w3x"));
                                        if (File.Exists(OBBoxFile))
                                        {
                                            File.Delete(OBBoxFile);
                                            Console.Write(".");
                                        }
                                    }
                                }

                                if (Meshes.Count != 0)
                                {
                                    foreach (string Mesh in Meshes)
                                    {
                                        string MeshFile = Path.Combine(fPath, (Mesh + ".w3x"));
                                        Console.Write(".");
                                        XmlTextReader reader = new XmlTextReader(MeshFile);
                                        while (reader.Read())
                                        {
                                            reader.ReadToFollowing("W3DMesh");
                                            string strInner = reader.ReadOuterXml();
                                            if (strInner.Length != 0)
                                            {
                                                XmlTextReader xmlReader = new XmlTextReader(new StringReader(strInner));
                                                XmlNode MeshNode = xDoc.ReadNode(xmlReader);
                                                xDoc.DocumentElement.InsertBefore(MeshNode, W3DContainer);
                                                xDoc.Save(nXML);
                                                Console.Write(".");
                                            }
                                        }
                                        reader.Close();
                                    }
                                    foreach (string Mesh in Meshes)
                                    {
                                        string MeshFile = Path.Combine(fPath, (Mesh + ".w3x"));
                                        if (File.Exists(MeshFile))
                                        {
                                            File.Delete(MeshFile);
                                            Console.Write(".");
                                        }
                                    }
                                }
                            }
                        }
                        XmlNodeList Textures = xDoc.GetElementsByTagName("Texture");
                        foreach (XmlNode Texture in Textures)
                        {
                            string strTexture = (string)Texture.InnerText;
                            strTexture = strTexture.Trim();
                            if (!TexturesList.Contains(strTexture))
                            {
                                TexturesList.Add(strTexture);
                                Console.Write(".");
                            }
                        }
                        foreach (string strTex in TexturesList)
                        {
                            XmlElement Include = xDoc.CreateElement("Include", "uri:ea.com:eala:asset");
                            Include.SetAttribute("type", "all");
                            string strTex2 = strTex.ToLower();
                            Include.SetAttribute("source", "ART:" + strTex2 + ".xml");
                            newIncludes.AppendChild(Include);
                            xDoc.Save(nXML);
                            Console.Write(".");
                        }
                        Console.Write("[SUCCESS]\n");
                    }
                }
                catch (Exception e)
                {
                    Console.Write("[ERROR]\n");
                    EmitError("Failed with exception: {0}" + Convert.ToString(e));
                }
            }
        }

        static void buildskn(string fPath)
        {
            DirectoryInfo directory = new DirectoryInfo(fPath);
            string[] w3xfiles = Directory.GetFiles(fPath, "*.w3x");
            foreach (string w3xfile in w3xfiles)
            {
                buildfile(w3xfile, fPath);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("MakeSKN will scan provided w3x containers and try to restore them.\nUsage: MakeSKN [Path]");
                }
                else
                {
                    if (args.Length != 1)
                    {
                        args = new string[1];
                        args[0] = "";
                    }
                    if (args[0].Contains(".w3x"))
                    {
                        Console.WriteLine("MakeSKN will process single file");
                        var directoryFullPath = Path.GetDirectoryName(args[0]);
                        buildfile(args[0], directoryFullPath);
                    }
                    else
                    {
                        Console.WriteLine("MakeSKN will process directory"); 
                        buildskn(args[0]);
                    }
                }

                if (errors > 0)
                {
                    Console.WriteLine("Errors: {0}", errors);
                }

            }
            catch (Exception e)
            {
                EmitError("Failed with exception: {0}"+Convert.ToString(e));
            }
        }
    }
}
