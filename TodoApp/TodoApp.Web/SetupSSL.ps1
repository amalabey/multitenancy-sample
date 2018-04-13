<#
.SYPOSIS:
	Generates and trusts a SSL certificate for local development. This script will generate (if cert does not exist) and import
	the self-signed certificate to certificate store.
#>
param([string]$Password = "1qaz2wsx@", [string]$DomainSuffix = "localtest.me")

$pfxPassword = ConvertTo-SecureString -String $Password -Force -AsPlainText
$pfxFilePath = "$($PSScriptRoot)\$($DomainSuffix)-ssl.pfx"
$cerFilePath = "$($PSScriptRoot)\$($DomainSuffix)-ssl.cer"

# Default cert available with the code-base
$certificatePath = "Cert:\CurrentUser\My\a67292ea177886b9fe170c9e4217b866f10a99c3"

if(-Not (Test-Path  ("$($PSScriptRoot)\$($DomainSuffix)-ssl.pfx")))
{
	# setup certificate properties including the commonName (DNSName) property for Chrome 58+
	$certificate = New-SelfSignedCertificate `
		-Subject "*.$($DomainSuffix)" `
		-DnsName "*.$($DomainSuffix)" `
		-KeyAlgorithm RSA `
		-KeyLength 2048 `
		-NotBefore (Get-Date) `
		-NotAfter (Get-Date).AddYears(10) `
		-CertStoreLocation "cert:CurrentUser\My" `
		-FriendlyName "Localhost Certificate for InnovationHub" `
		-HashAlgorithm SHA256 `
		-KeyUsage DigitalSignature, KeyEncipherment, DataEncipherment `
		-TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1") 
	$certificatePath = 'Cert:\CurrentUser\My\' + ($certificate.ThumbPrint)  

	# Export certs, so that we can import them to root
	Export-PfxCertificate -Cert $certificatePath -FilePath $pfxFilePath -Password $pfxPassword
	Export-Certificate -Cert $certificatePath -FilePath $cerFilePath
	Import-PfxCertificate -FilePath $pfxFilePath Cert:\LocalMachine\My -Password $pfxPassword -Exportable
}else
{
	# Import the pfx from source and get the public-key cert out
	Import-PfxCertificate -FilePath $pfxFilePath Cert:\LocalMachine\My -Password $pfxPassword -Exportable
	Export-Certificate -Cert $certificatePath -FilePath $cerFilePath
}

# trust the certificate by importing the pfx certificate into your trusted root
Import-Certificate -FilePath $cerFilePath -CertStoreLocation Cert:\CurrentUser\Root

# optionally delete the physical certificates (don’t delete the pfx file as you need to copy this to your app directory)
# Remove-Item $pfxFilePath
Remove-Item $cerFilePath