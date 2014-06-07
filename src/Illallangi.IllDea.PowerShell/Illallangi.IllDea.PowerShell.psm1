if ($null -eq (Get-Module PSCompletion))
{
	Write-Debug "Import-Module PSCompletion -Global"
	Import-Module PSCompletion -Global -ErrorAction SilentlyContinue
	if ($null -eq (Get-Module PSCompletion))
	{
		Write-Warning "PSCompletion module not found; tab completion will be unavailable."
	}
}

if ($null -ne (Get-Module PSCompletion))
{
	Register-ParameterCompleter Open-Company Name {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Company -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter Get-Account Name {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}
	
	Register-ParameterCompleter Get-Account Number {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter Get-Account Id {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter Remove-Account Name {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}
	
	Register-ParameterCompleter Remove-Account Number {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter Remove-Account Id {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter Get-Period Start {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Period | Where-Object { $_.Start.ToString("yyyy-MM-dd") -like "$wordToComplete*" } | Sort-Object { $_.Start } |%{ New-CompletionResult "$($_.Start.ToString("yyyy-MM-dd"))" }
	}

	Register-ParameterCompleter Get-Period End {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Period | Where-Object { $_.End.ToString("yyyy-MM-dd") -like "$wordToComplete*" } | Sort-Object { $_.End } |%{ New-CompletionResult "$($_.End.ToString("yyyy-MM-dd"))" }
	}

	Register-ParameterCompleter Get-Period Id {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Period | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter Remove-Period Start {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Period | Where-Object { $_.Start.ToString("yyyy-MM-dd") -like "$wordToComplete*" } | Sort-Object { $_.Start } |%{ New-CompletionResult "$($_.Start.ToString("yyyy-MM-dd"))" }
	}

	Register-ParameterCompleter Remove-Period End {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Period | Where-Object { $_.End.ToString("yyyy-MM-dd") -like "$wordToComplete*" } | Sort-Object { $_.End } |%{ New-CompletionResult "$($_.End.ToString("yyyy-MM-dd"))" }
	}

	Register-ParameterCompleter Remove-Period Id {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Period | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter Get-Txn Description {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn -Description "$wordToComplete*" | Sort-Object { $_.Description } |%{ New-CompletionResult """$($_.Description)""" }
	}

	Register-ParameterCompleter Get-Txn Date {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn | Where-Object { $_.Date.ToString("yyyy-MM-dd") -like "$wordToComplete*" } | Sort-Object { $_.Date } |%{ New-CompletionResult "$($_.Date.ToString("yyyy-MM-dd"))" }
	}

	Register-ParameterCompleter Get-Txn Id {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter Remove-Txn Description {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn -Description "$wordToComplete*" | Sort-Object { $_.Description } |%{ New-CompletionResult """$($_.Description)""" }
	}

	Register-ParameterCompleter Remove-Txn Date {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn | Where-Object { $_.Date.ToString("yyyy-MM-dd") -like "$wordToComplete*" } | Sort-Object { $_.Date } |%{ New-CompletionResult "$($_.Date.ToString("yyyy-MM-dd"))" }
	}

	Register-ParameterCompleter Remove-Txn Id {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter Add-TxnItem AccountName {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter Add-TxnItem AccountNumber {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter Add-TxnItem AccountId {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter New-Employee SalaryExpenseAccountName {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter New-Employee SalaryExpenseAccountNumber {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter New-Employee SalaryExpenseAccountId {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter New-Employee EmployeeLiabilityAccountName {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter New-Employee EmployeeLiabilityAccountNumber {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter New-Employee EmployeeLiabilityAccountId {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter New-Employee IncomeTaxLiabilityAccountName {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter New-Employee IncomeTaxLiabilityAccountNumber {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter New-Employee IncomeTaxLiabilityAccountId {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter New-Employee SuperannuationExpenseAccountName {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter New-Employee SuperannuationExpenseAccountNumber {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter New-Employee SuperannuationExpenseAccountId {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}

	Register-ParameterCompleter New-Employee SuperannuationLiabilityAccountName {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter New-Employee SuperannuationLiabilityAccountNumber {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Number.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Number } |%{ New-CompletionResult """$($_.Number)""" }
	}

	Register-ParameterCompleter New-Employee SuperannuationLiabilityAccountId {
	  param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
	  Get-Account | Where-Object { $_.Id.ToString() -like "$wordToComplete*" } | Sort-Object { $_.Id } |%{ New-CompletionResult """$($_.Id.ToString())""" }
	}
}