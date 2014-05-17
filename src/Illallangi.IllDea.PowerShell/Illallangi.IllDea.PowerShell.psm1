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

	Register-ParameterCompleter Add-TxnItem AccountName {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Account -CompanyId (Get-Variable Dea* -Scope global).Value.Id -Name "$wordToComplete*" | Sort-Object { $_.Name } |%{ New-CompletionResult """$($_.Name)""" }
	}

	Register-ParameterCompleter Get-Txn Description {
		param($commandName, $parameterName, $wordToComplete, $commandAst, $fakeBoundParameter)
		Get-Txn -CompanyId (Get-Variable Dea* -Scope global).Value.Id -Description "$wordToComplete*" | Sort-Object { $_.Description } |%{ New-CompletionResult """$($_.Description)""" }
	}
}