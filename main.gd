extends Node

func _on_tile_grid_playing_state_changed(IsRunning):
	$GUI.SetPlayingState(IsRunning)


func _on_gui_play_button_pressed(PlaySpeed):
	$TileGrid.SetPlayingState(PlaySpeed)


func _on_gui_reset_button_pressed():
	$TileGrid.ResetAllTiles()
