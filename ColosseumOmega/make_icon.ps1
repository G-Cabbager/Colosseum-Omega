# 将源 PNG 转换为多尺寸 .ico(PNG 条目,Vista+ 兼容)
# 用法: powershell -ExecutionPolicy Bypass -File make_icon.ps1 -Png iconic.png -OutIco app.ico
param(
    [string]$Png = 'iconic.png',
    [string]$OutIco = 'app.ico'
)

Add-Type -AssemblyName System.Drawing

$srcPath = Join-Path $PSScriptRoot $Png
$outPath = Join-Path $PSScriptRoot $OutIco
$src = [System.Drawing.Image]::FromFile($srcPath)
if (-not $src) { throw "无法加载图片: $srcPath" }

$sizes = @(256, 128, 64, 48, 32, 24, 16)
$pngs = New-Object System.Collections.Generic.List[byte[]]

foreach ($s in $sizes) {
    $bmp = New-Object System.Drawing.Bitmap($s, $s)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $g.Clear([System.Drawing.Color]::Transparent)
    $g.DrawImage($src, (New-Object System.Drawing.Rectangle(0, 0, $s, $s)))
    $g.Dispose()

    $ms = New-Object System.IO.MemoryStream
    $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
    $pngs.Add($ms.ToArray())
    $ms.Dispose()
}
$src.Dispose()

# 组装 ICO 文件
$fs = [System.IO.File]::Create($outPath)
$bw = New-Object System.IO.BinaryWriter($fs)

$count = $sizes.Count
$bw.Write([uint16]0)                      # reserved
$bw.Write([uint16]1)                      # type: icon
$bw.Write([uint16]$count)

$offset = 6 + 16 * $count
for ($i = 0; $i -lt $count; $i++) {
    $w = $sizes[$i]
    $entryWidth  = 0; if ($w -lt 256) { $entryWidth  = $w }  # 0 表示 256
    $entryHeight = 0; if ($w -lt 256) { $entryHeight = $w }
    $bw.Write([byte]$entryWidth)
    $bw.Write([byte]$entryHeight)
    $bw.Write([byte]0)                    # colorCount
    $bw.Write([byte]0)                    # reserved
    $bw.Write([uint16]1)                  # planes
    $bw.Write([uint16]32)                 # bitCount
    $data = $pngs[$i]
    $bw.Write([uint32]$data.Length)       # bytesInRes
    $bw.Write([uint32]$offset)            # imageOffset
    $offset += $data.Length
}

foreach ($d in $pngs) { $bw.Write($d) }
$bw.Flush()
$bw.Close()
$fs.Close()

Write-Output "OK 已生成: $outPath (尺寸: $($sizes -join ','))"
