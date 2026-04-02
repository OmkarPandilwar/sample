namespace CustomerManagement.Application.DTOs;

public record SegmentationDto(
    Dictionary<string, int> BySegment,
    Dictionary<string, int> ByClassification,
    Dictionary<string, int> ByType,
    int Total,
    DateTime GeneratedAt
);