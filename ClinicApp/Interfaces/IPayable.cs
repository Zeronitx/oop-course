namespace ClinicApp.Interfaces;

public interface IPayable
{
    bool IsPaid { get; }
    decimal GetCost();
    void MarkPaid();
}