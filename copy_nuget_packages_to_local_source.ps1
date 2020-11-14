$src_root = Resolve-Path("./src/")
$target_root = Resolve-Path("D:\nuget_source\Groundbeef\")
$glob_filter = "*.nupkg"
Get-ChildItem -path $src_root -filter $glob_filter -recurse | 
  ForEach-Object {
        $target_filename=Join-Path -Path $target_root -ChildPath (Split-Path $_.FullName -Leaf)  #($_.FullName -replace [regex]::escape($src_root),$target_root) 
        $target_path=Split-Path $target_filename
        if (!(Test-Path -Path $target_path)) {
            New-Item $target_path -ItemType Directory
        }
        Copy-Item $_.FullName -destination $target_filename
    }