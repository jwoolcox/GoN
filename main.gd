extends Node

# Called when the node enters the scene tree for the first time.
func _ready():
	$AudioStreamPlayer.play() 
	


func _on_tile_grid_playing_state_changed(IsRunning):
	$GUI.SetPlayingState(IsRunning)


func _on_gui_play_button_pressed(PlaySpeed):
	$TileGrid.SetPlayingState(PlaySpeed)


func _on_gui_reset_button_pressed():
	pass # Replace with function body.
