$src_root = Resolve-Path("./src/")
$target_root = Resolve-Path("D:\nuget_source\")
$glob_filter = "*.nupkg"
Get-ChildItem -path $src_root -filter $glob_filter -recurse | 
  ForEach-Object {
        $target_filename=Join-Path -Path $target_root -ChildPath (Split-Path $_.FullName -Leaf)
        Copy-Item $_.FullName -destination $target_filename
    }