using System;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Prescription : MedicalRecord
{
    private string _medicationName = "";
    public string MedicationName
    {
        get => _medicationName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(MedicationName));
            _medicationName = value;
        }
    }

    private string _dosage = "";
    public string Dosage
    {
        get => _dosage;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Dosage));
            _dosage = value;
        }
    }

    private int _durationDays;
    public int DurationDays
    {
        get => _durationDays;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationDays));
            _durationDays = value;
        }
    }

    public string Instructions { get; set; } = "";

    public DateTime ExpiresAt => Date.AddDays(DurationDays);

    public Prescription(int patientId, int doctorId, DateTime date, string medicationName, string dosage, int durationDays, string instructions = "")
        : base(patientId, doctorId, date)
    {
        MedicationName = medicationName;
        Dosage = dosage;
        DurationDays = durationDays;
        Instructions = instructions;
    }

    public override string GetSummary()
    {
        string instructionPart = !string.IsNullOrEmpty(Instructions) ? $" ({Instructions})" : "";
        return $"{MedicationName} {Dosage} × {DurationDays} днів{instructionPart}";
    }

    public override string GetRecordType()
    {
        return "Рецепт";
    }

    public override bool IsActive()
    {
        return ExpiresAt >= DateTime.Today;
    }
}