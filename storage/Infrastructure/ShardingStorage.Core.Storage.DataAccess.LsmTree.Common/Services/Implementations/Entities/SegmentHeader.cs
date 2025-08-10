using System.Globalization;
using System.Text;

namespace ShardingStorage.Core.Storage.DataAccess.LsmTree.Common.Services.Implementations.Entities;

public record SegmentHeader(long Count, DateTime CreationDate, int BytesPerBlock, string CompressorName)
{
    public static SegmentHeader ReadFromFile(string filename)
    {
        using var reader =
            new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read),
                Encoding.UTF8, false);

        return GetHeaderFromReader(reader);
    }

    public void WriteWithBlockMap(string filename, List<(string, long)> blocks)
    {
        using var writer = new BinaryWriter(new FileStream(filename, FileMode.Create), Encoding.UTF8, false);

        writer.Write(Count);
        writer.Write(CreationDate.ToString(CultureInfo.InvariantCulture));
        writer.Write(BytesPerBlock);
        writer.Write(CompressorName);

        foreach (var block in blocks)
        {
            writer.Write(block.Item1);
            writer.Write(block.Item2);
        }
    }

    public static void SkipHeaders(BinaryReader reader)
    {
        var _ = GetHeaderFromReader(reader);
    }

    private static SegmentHeader GetHeaderFromReader(BinaryReader reader)
    {
        var count = reader.ReadInt64();
        var date = DateTime.Parse(reader.ReadString(), CultureInfo.InvariantCulture);
        var bytes = reader.ReadInt32();
        var compressor = reader.ReadString();

        return new SegmentHeader(count, date, bytes, compressor);
    }
}