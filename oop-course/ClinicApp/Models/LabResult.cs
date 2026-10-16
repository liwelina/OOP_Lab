using System;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class LabResult : MedicalRecord
{
    private string _testName = "";
    public string TestName
    {
        get => _testName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(TestName));
            _testName = value;
        }
    }

    public double Value { get; set; }

    private string _unit = "";
    public string Unit
    {
        get => _unit;
        set
        {
            ClinicValidator.ValidateName(value, nameof(Unit));
            _unit = value;
        }
    }

    private string _referenceRange = "";
    public string ReferenceRange
    {
        get => _referenceRange;
        set
        {
            ClinicValidator.ValidateName(value, nameof(ReferenceRange));
            _referenceRange = value;
        }
    }

    public bool IsNormal { get; set; }

    public LabResult(int patientId, int doctorId, DateTime date, string testName, double value, string unit, string referenceRange, bool isNormal)
        : base(patientId, doctorId, date)
    {
        TestName = testName;
        Value = value;
        Unit = unit;
        ReferenceRange = referenceRange;
        IsNormal = isNormal;
    }

    public override string GetSummary()
    {
        string statusSuffix = !IsNormal ? " ⚠ поза нормою" : "";
        return $"{TestName}: {Value} {Unit} (норма: {ReferenceRange}){statusSuffix}";
    }

    public override string GetRecordType()
    {
        return "Аналіз";
    }
}