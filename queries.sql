--TO GET businesses STRINGS:
--This doesn't really work as .sql as C# modifies the query depending on what needs to be there

SELECT DISTINCT name,full_address,review_count,numcheckins,business_id FROM business " + where + ";";
--Depending on what is necessary ^where will contain:
sb.Append("WHERE ");
sb.Append("state='" + statesComboBox.SelectedItem + "' ");
sb.Append("city='" + citiesListBox.SelectedItem + "' ");
sb.Append("business_id IN (SELECT business_id FROM categories WHERE ");
sb.Append("category = '" + selectedCategoriesListBox.Items[i] + "' ");
sb.Append("RIGHT(full_address,5)='" + zipCodeListBox.SelectedItem + "' ");
sb.Append("business_id IN (SELECT business_id FROM hours WHERE open <= CAST('" + fromComboBox.SelectedItem + "' AS TIME) AND close >= CAST('" + toComboBox.SelectedItem + "' AS TIME) AND day='" + dayOfTheWeekComboBox.SelectedItem + "')");
sb.Append("AND business_id IN (SELECT business_id FROM hours WHERE open=close AND day='" + dayOfTheWeekComboBox.SelectedItem + "')");

--FOR TIPS
cmd.CommandText = "SELECT name,thedate,likes,txt FROM Tip INNER JOIN users ON tip.business_id = '" + b.id + "' AND tip.user_id = users.user_id ";

				


