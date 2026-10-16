using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
	private const int MaxCapacity = 1000;
	private readonly MedicalRecord[] _records = new MedicalRecord[MaxCapacity];
	private int _count = 0;

	public int Count => _count;

	public MedicalRecord? this[int index]
	{
		get
		{
			if (index < 0 || index >= _count)
			{
				return null;
			}
			return _records[index];
		}
	}

	public void Add(MedicalRecord record)
	{
		if (_count >= MaxCapacity)
		{
			Console.WriteLine("Помилка: досягнуто максимальну кількість медичних записів.");
			return;
		}

		_records[_count++] = record;
		Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
	}

	public MedicalRecord? FindById(int id)
	{
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].Id == id)
			{
				return _records[i];
			}
		}
		return null;
	}

	public MedicalRecord[] GetByPatient(int patientId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId)
			{
				matchCount++;
			}
		}

		MedicalRecord[] result = new MedicalRecord[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId)
			{
				result[index++] = _records[i];
			}
		}

		return result;
	}

	public MedicalRecord[] GetByDoctor(int doctorId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].DoctorId == doctorId)
			{
				matchCount++;
			}
		}

		MedicalRecord[] result = new MedicalRecord[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].DoctorId == doctorId)
			{
				result[index++] = _records[i];
			}
		}

		return result;
	}

	public Diagnosis[] GetDiagnoses(int patientId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
			{
				matchCount++;
			}
		}

		Diagnosis[] result = new Diagnosis[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Diagnosis diag)
			{
				result[index++] = diag;
			}
		}

		return result;
	}

	public LabResult[] GetLabResults(int patientId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is LabResult)
			{
				matchCount++;
			}
		}

		LabResult[] result = new LabResult[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is LabResult lab)
			{
				result[index++] = lab;
			}
		}

		return result;
	}

	public Prescription[] GetPrescriptions(int patientId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Prescription)
			{
				matchCount++;
			}
		}

		Prescription[] result = new Prescription[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Prescription pres)
			{
				result[index++] = pres;
			}
		}

		return result;
	}

	public Diagnosis[] GetChronicDiagnoses(int patientId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Diagnosis diag && diag.IsChronic)
			{
				matchCount++;
			}
		}

		Diagnosis[] result = new Diagnosis[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Diagnosis diag && diag.IsChronic)
			{
				result[index++] = diag;
			}
		}

		return result;
	}

	public Prescription[] GetActivePrescriptions(int patientId)
	{
		int matchCount = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Prescription pres && pres.IsActive())
			{
				matchCount++;
			}
		}

		Prescription[] result = new Prescription[matchCount];
		int index = 0;
		for (int i = 0; i < _count; i++)
		{
			if (_records[i].PatientId == patientId && _records[i] is Prescription pres && pres.IsActive())
			{
				result[index++] = pres;
			}
		}

		return result;
	}

	public void DisplayPatientSummary(int patientId)
	{
		MedicalRecord[] patientRecords = GetByPatient(patientId);

		if (patientRecords.Length == 0)
		{
			Console.WriteLine($"Для пацієнта #{patientId} немає жодних медичних записів.");
			return;
		}

		int diagCount = 0;
		int labCount = 0;
		int prescCount = 0;

		for (int i = 0; i < patientRecords.Length; i++)
		{
			if (patientRecords[i] is Diagnosis) diagCount++;
			else if (patientRecords[i] is LabResult) labCount++;
			else if (patientRecords[i] is Prescription) prescCount++;
		}

		Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
		Console.WriteLine($"Всього записів: {patientRecords.Length} (діагнозів: {diagCount}, аналізів: {labCount}, рецептів: {prescCount})");

		Diagnosis[] chronic = GetChronicDiagnoses(patientId);
		if (chronic.Length > 0)
		{
			Console.WriteLine($"Хронічні діагнози ({chronic.Length}):");
			for (int i = 0; i < chronic.Length; i++)
			{
				Console.WriteLine($"  {chronic[i]}");
			}
		}

		Prescription[] activePresc = GetActivePrescriptions(patientId);
		if (activePresc.Length > 0)
		{
			Console.WriteLine($"Активні рецепти ({activePresc.Length}):");
			for (int i = 0; i < activePresc.Length; i++)
			{
				Console.WriteLine($"  {activePresc[i]} | до {activePresc[i].ExpiresAt:dd.MM.yyyy}");
			}
		}
	}

	public void DisplayAll()
	{
		if (_count == 0)
		{
			Console.WriteLine("Немає жодного медичного запису.");
			return;
		}

		DisplayList(_records);
	}

	public void DisplayList(MedicalRecord[] records)
	{
		if (records.Length == 0)
		{
			Console.WriteLine("Список записів порожній.");
			return;
		}

		for (int i = 0; i < records.Length; i++)
		{
			Console.WriteLine(records[i]);
		}
	}
}