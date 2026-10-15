namespace Tradelens.Auditing.Domain;

/// <summary>
/// Consider the Observation entity in the CompanyFundamentals bounded context (BC),
/// is an ObservationAudit in the Auditing BC, which can
/// possibly be an IAudit or IAuditable, keeping only the attributes and behavior that are necessary/relevant to the
/// Auditing BC.
/// https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-domain-model
/// </summary>
public interface IAudit
{
    void Submit();
}