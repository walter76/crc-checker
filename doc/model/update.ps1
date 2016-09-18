java -jar "C:\home\bin\plantuml\plantuml.jar" $args[0]

[void][reflection.assembly]::LoadWithPartialName("System.Windows.Forms")

$filename = [Io.Path]::GetFileNameWithoutExtension($args[0]) + ".png"
$file = (get-item $filename)

$img = [System.Drawing.Image]::Fromfile($file);

[System.Windows.Forms.Application]::EnableVisualStyles();
$form = new-object Windows.Forms.Form
$form.Text = "Image Viewer"
$form.Width = $img.Size.Width + 15;
$form.Height = $img.Size.Height + 35;
$pictureBox = new-object Windows.Forms.PictureBox
$pictureBox.Width = $img.Size.Width;
$pictureBox.Height = $img.Size.Height;

$pictureBox.Image = $img;
$form.controls.add($pictureBox)
$form.Add_Shown( { $form.Activate() } )
$form.ShowDialog()
