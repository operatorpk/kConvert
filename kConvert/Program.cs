using NAudio;
using NAudio.Lame;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kConvert
{
    class Program
    {
        //logger
        private static readonly Logger logger = LogManager.GetLogger(Helpers.GetAppName());

        static void Main(string[] args)
        {
            logger.Info("------------------------------------------------------------------");
            logger.Info("{name} v{version} started...", Helpers.GetAppName(), Helpers.GetAppVersion());

            if (args.Length < 1)
            {
                logger.Error("we need at least one argument: source directory. But we got {count} arguments.", args.Length);
                logger.Info("   usage: kConvert <source director> <optional: destination directory>");
                //Console.WriteLine("we need at least one argument: source directory. But we got " + args.Length.ToString() + " arguments.");
            }
            else if(args.Length == 1)
            { 
                string sourceDirectory = args[0].Trim();
                logger.Info("using source directory {source}", sourceDirectory);
                //Console.WriteLine("using source directory " + sourceDirectory);
                ProcessDirectory(sourceDirectory, sourceDirectory);
            }
            else if (args.Length == 2)
            {
                string sourceDirectory = args[0].Trim();
                string targetDirectory = args[1].Trim();
                
                logger.Info("using source directory {source}", sourceDirectory);
                //Console.WriteLine("using source directory " + sourceDirectory);
                logger.Info("using target directory {target}", targetDirectory);
                //Console.WriteLine("using target directory " + targetDirectory);

                ProcessDirectory(sourceDirectory, targetDirectory);
            }
        }

        //process all wma files in given directory
        private static void ProcessDirectory(string sourceDirectory, string outputDirectory)
        {
            logger.Debug("ProcessDirectory({sourceDirectory}, {outputDirectory}...", sourceDirectory, outputDirectory);
            
            //get wma files in source directory
            string[] wmaFiles = Directory.GetFiles(sourceDirectory, "*.wma", System.IO.SearchOption.TopDirectoryOnly);
            if (wmaFiles.Length == 0)
            {
                logger.Error("no wma files found in source directory. Quitting.");
                return;
            }

            //loop wma files
            for (int i = 0; i < wmaFiles.Length; i++)
            {
                logger.Debug("processing file {no} of {total}", i + 1, wmaFiles.Length);
                logger.Debug("reading wma file {name}", wmaFiles[i]);
                string outputFile = Path.Combine(outputDirectory, Path.GetFileName(wmaFiles[i])).Replace(".wma", ".mp3");
                logger.Debug("new file is {newfile}", outputFile);

                //read metadata from source file
                var tagFile = TagLib.File.Create(wmaFiles[i]);
                if (tagFile.Tag != null)
                {
                    logger.Info("read metadata: {artist} - {title}", tagFile.Tag.FirstPerformer, tagFile.Tag.Title);
                }

                //convert wma file to mp3 file
                ConvertToMP3(wmaFiles[i], outputFile);

                //finally write tag data to new file
                if (tagFile.Tag != null)
                {
                    var newTagFile = TagLib.File.Create(outputFile);
                    newTagFile.Tag.Album = tagFile.Tag.Album;
                    newTagFile.Tag.AlbumArtists = tagFile.Tag.AlbumArtists;
                    newTagFile.Tag.AlbumArtistsSort = tagFile.Tag.AlbumArtistsSort;
                    newTagFile.Tag.AlbumSort = tagFile.Tag.AlbumSort;
                    newTagFile.Tag.AmazonId = tagFile.Tag.AmazonId;
                    newTagFile.Tag.BeatsPerMinute = tagFile.Tag.BeatsPerMinute;
                    newTagFile.Tag.Comment = tagFile.Tag.Comment;
                    newTagFile.Tag.Composers = tagFile.Tag.Composers;
                    newTagFile.Tag.ComposersSort = tagFile.Tag.ComposersSort;
                    newTagFile.Tag.Conductor = tagFile.Tag.Conductor;
                    newTagFile.Tag.Copyright = tagFile.Tag.Copyright;
                    newTagFile.Tag.DateTagged = tagFile.Tag.DateTagged;
                    newTagFile.Tag.Description = tagFile.Tag.Description;
                    newTagFile.Tag.Disc = tagFile.Tag.Disc;
                    newTagFile.Tag.DiscCount = tagFile.Tag.DiscCount;
                    newTagFile.Tag.Genres = tagFile.Tag.Genres;
                    newTagFile.Tag.Grouping = tagFile.Tag.Grouping;
                    newTagFile.Tag.InitialKey = tagFile.Tag.InitialKey;
                    newTagFile.Tag.ISRC = tagFile.Tag.ISRC;
                    newTagFile.Tag.Lyrics = tagFile.Tag.Lyrics;
                    newTagFile.Tag.Performers = tagFile.Tag.Performers;
                    newTagFile.Tag.PerformersRole = tagFile.Tag.PerformersRole;
                    newTagFile.Tag.PerformersSort = tagFile.Tag.PerformersSort;
                    newTagFile.Tag.Pictures = tagFile.Tag.Pictures;
                    newTagFile.Tag.Publisher = tagFile.Tag.Publisher;
                    newTagFile.Tag.RemixedBy = tagFile.Tag.RemixedBy;
                    newTagFile.Tag.Subtitle = tagFile.Tag.Subtitle;
                    newTagFile.Tag.Title = tagFile.Tag.Title;
                    newTagFile.Tag.TitleSort = tagFile.Tag.TitleSort;
                    newTagFile.Tag.Track = tagFile.Tag.Track;
                    newTagFile.Tag.TrackCount = tagFile.Tag.TrackCount;
                    newTagFile.Tag.Year = tagFile.Tag.Year;
                    newTagFile.Save(); 
                }

            }

            logger.Info("Finished converting files.");
            //Console.WriteLine("Finished converting files.");
        }

        //convert source audio file to mp3 format
        private static void ConvertToMP3(string sourceFilename, string targetFilename)
        {
            logger.Debug("ConvertToMP3({sourceFilename}, {targetFilename})...", sourceFilename, targetFilename);
            try
            {
                using (var reader = new NAudio.Wave.AudioFileReader(sourceFilename))
                using (var writer = new NAudio.Lame.LameMP3FileWriter(targetFilename, reader.WaveFormat, NAudio.Lame.LAMEPreset.STANDARD))
                {
                    reader.CopyTo(writer);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Could not convert file {source} to {target}", sourceFilename, targetFilename);
            }
        }
    }
}
