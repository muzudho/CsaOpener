#############
# Locations #
#############

$original = "C:\Users\むずでょ\source\repos\shogi-kifu-converter"
$deploy   = "C:\Users\むずでょ\Documents\GitHub\shogi-kifu-converter";

#######
# Lib #
#######

# ファイルの削除。
function Remove-File($dst) {
	if (Test-Path $dst) {
		Write-Host "Delete  : [$($dst)]."
		Remove-Item $dst
	}
}

# ディレクトリーの削除。
function Remove-Dir($dst) {
	if (Test-Path $dst) {
		Write-Host "Delete  : [$($dst)] directory."
		Remove-Item $dst -Recurse
	}
}

# ファイルのコピー。
function Copy-File($src, $dst) {
	Write-Host "Copy    : [$($src)] --> [$($dst)]."
	Copy-Item $src $dst
}

# ディレクトリーのコピー。
function Copy-Dir($src, $dst) {
	Write-Host "Copy    : [$($src)] --> [$($dst)] directory."
	Copy-Item $src $dst -Recurse
}

# ファイルの移動。
function Move-File($src, $dst) {
	if (Test-Path $src) {
		Write-Host "Move    : [$($src)] --> [$($dst)]."
		Move-Item $src $dst
	}
}

# ディレクトリーの移動。
function Move-File($src, $dst) {
	if (Test-Path $src) {
		Write-Host "Move    : [$($src)] --> [$($dst)]."
		Move-Item $src $dst -Recurse
	}
}

######
# Go #
######

##
 # Note: Not trailer slash.
 ##

Remove-Dir  "$($deploy)\-- GENERATOR"
Remove-Dir  "$($deploy)\visual-studio"

Remove-File "$($deploy)\.gitignore"
Remove-File "$($deploy)\LICENSE"
Remove-File "$($deploy)\README.md"

Copy-Dir    "$($original)\-- GENERATOR"                      "$($deploy)\-- GENERATOR"
Copy-Dir    "$($original)\visual-studio"                     "$($deploy)\visual-studio"

Copy-File   "$($original)\.gitignore"                        "$($deploy)\.gitignore"
Copy-File   "$($original)\LICENSE"                           "$($deploy)\LICENSE"
Copy-File   "$($original)\README.md"                         "$($deploy)\README.md"

pause