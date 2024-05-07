export function dragStart(event) {
    event.dataTransfer.setData("text/plain", event.target.id);
}

export function dragOver(event) {
    event.preventDefault();
}

export async function drop(event) {
    event.preventDefault();
    var data = await event.dataTransfer.getData("text/plain");
    var droppedItemId = data;
    var targetItemId = event.target.id;

    // Get the index of the dropped item and the target item
    var droppedItemIndex = findItemIndexById(droppedItemId);
    var targetItemIndex = findItemIndexById(targetItemId);

    // Ensure both items exist and are not the same
    if (droppedItemIndex !== -1 && targetItemIndex !== -1 && droppedItemIndex !== targetItemIndex) {
        // Remove the dropped item from the list
        var droppedItem = ListItems.splice(droppedItemIndex, 1)[0];

        // Insert the dropped item at the target index
        ListItems.splice(targetItemIndex, 0, droppedItem);

        // Update the UI by re-rendering the list with the updated order
        renderListItems();
    }
}

function findItemIndexById(itemId) {
    // Implement a function to find the index of an item in your list by its ID
    // This function depends on how your list data is structured
    // You may need to iterate through the list and compare IDs
    // Return -1 if the item is not found
    for (var i = 0; i < ListItems.length; i++) {
        if (ListItems[i].id === itemId) {
            return i;
        }
    }
    return -1;
}

function renderListItems() {
    // Implement a function to re-render the list with the updated order
    // This function should update the UI to reflect the new order of list items
    // You may need to use a framework or library to update the UI
    // For example, if you're using React, you would update state and let React handle the re-rendering
}

