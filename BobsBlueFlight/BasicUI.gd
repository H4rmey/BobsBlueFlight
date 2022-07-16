extends Control


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	$VBoxContainer/Startbutton.grab_focus()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

#Druk op de start knop ga naar de //BOBGAME scene
func _on_Startbutton_pressed():
	get_tree().change_scene("res://UI scenes/BOBGAME.tscn")

#Druk op de start knop ga naar de //Options Menu  scene
func _on_Options_pressed():
	get_tree().change_scene("res://UI scenes/Options Menu.tscn")
	


func _on_Lore_pressed():
	get_tree().change_scene("res://UI scenes/Lore.tscn")
	


func _on_Credits_pressed():
	get_tree().change_scene("res://UI scenes/Credits.tscn")
	



func _on_Quit_pressed():
	get_tree().quit()
