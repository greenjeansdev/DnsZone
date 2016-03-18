$baseDir = resolve-path ..
$srcPath = $baseDir;
$solutionPath = "$baseDir\DnsZone.sln";
$buildDir = "$baseDir\build"
$packageDir = "$buildDir\package"

$buildNuGet = $true;
$nugetPackageId = "DnsZone";
$nugetVersion = "1.0.0";
$nugetSourcePath = "$buildDir\dnszone.nuspec"
$nuget = "$buildDir\nuget\nuget.exe"
$nugetOutput = "$buildDir\output"

$msbuild = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\msbuild.exe"
$configuration = "Release"

$nugetSpec = "$packageDir\dnszone.nuspec"

function Ensure-Folder($path) {
	if(!(Test-Path -Path $path)){
		New-Item -ItemType directory -Path $path
	}
}

function Delete-Folder($path) {
	if(Test-Path -Path $path){
		Remove-Item $path -recurse
	}
}

function Parse-NuSpec($path) {
    $xml = [xml](Get-Content $path)
	$meta = $xml.package.metadata;
	@{
		Version = $meta.version;
	}
}

function Update-NuSpec($path) {
    $xml = [xml](Get-Content $path)
	$meta = $xml.package.metadata;
	$meta.id = $nugetPackageId;
	$meta.version = $nugetVersion;
    $xml.save($path)
}

function Pack-NuSpec ($path) {
	Ensure-Folder $nugetOutput
	& $nuget pack $path -Symbols -OutputDirectory $nugetOutput
}

function Build-Project($path) {
	& $msbuild $path /p:Configuration=$configuration
}

function Build-Solution($path) {
	& $nuget restore $path
	& $msbuild $path /p:Configuration=$configuration
}

function Copy-Build($projectDir)  {
	$binSourcePath = "$projectDir\bin\$configuration";
	$binTargetPath = "$packageDir\lib\net45"
	Ensure-Folder $binTargetPath
    Copy-Item -Path $binSourcePath\*.dll -Destination $binTargetPath -recurse
    Copy-Item -Path $binSourcePath\*.pdb -Destination $binTargetPath -recurse
    Copy-Item -Path $binSourcePath\*.xml -Destination $binTargetPath -recurse
}

Delete-Folder $packageDir
Delete-Folder $nugetOutput

Write-Host "Building DnsZone solution"
Build-Solution $solutionPath

Write-Host "Copying bin files to package"
Ensure-Folder $packageDir
Ensure-Folder "$packageDir\lib"
Copy-Build "$srcPath\DnsZone"

Write-Host "parsing nuspec file" -ForegroundColor Green
$nuspecInfo = Parse-NuSpec $nugetSourcePath
Write-Host "version: $($nuspecInfo.Version)"
$nugetVersion = $nuspecInfo.Version

Write-Host "Updating nuspec file at $nugetSpec" -ForegroundColor Green
Copy-Item -Path $nugetSourcePath -Destination $nugetSpec
Update-NuSpec $nugetSpec

Write-Host "Building NuGet package with ID $packageId and version $nugetVersion" -ForegroundColor Green
Pack-NuSpec($nugetSpec);
