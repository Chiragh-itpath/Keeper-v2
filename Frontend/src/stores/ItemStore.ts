import { ref, type Ref } from 'vue'
import { defineStore } from 'pinia'
import type { IItem, IAddItem, IEditItem } from '@/Models/ItemModels'
import { ItemService } from '@/Services/ItemService'
import type { IAddComment, IComment } from '@/Models/CommentModel'
import { useToster } from '@/composable/useToaster'
import type { ItemStatus } from '@/Models/enum'

const ItemStore = defineStore('item', () => {
    const Items: Ref<IItem[]> = ref([])
    const AllItems: Ref<IItem[]> = ref([])

    const KeepId: Ref<string> = ref('')
    const itemService = new ItemService()
    const getSingalItem = (id: string): IItem | undefined => {
        const item = Items.value.find((x) => x.id == id)
        return item
    }

    const GetAllItems = async (id: string): Promise<void> => {
        KeepId.value = id
        const response = await itemService.GetAll(id)
        if (response) {
            AllItems.value = response
            fetchItems()
        }
    }

    const GetById = async (id: string): Promise<IItem | null> => {
        const response = await itemService.GetById(id)
        if (response) {
            return response
        }
        return null
    }
    const CreateForm = (item: IAddItem | IEditItem): FormData => {
        const formData = new FormData();
        formData.append('type', String(item.type));
        for (const [key, value] of Object.entries(item)) {
            if (value !== null && value !== undefined && key !== 'files') {
                formData.append(key, String(value));
            }
        }
        if (item.files) {
            item.files.forEach((file) => {
                formData.append('files', file);
            });
        }
        return formData;
    };

    const AddItem = async (addItem: IAddItem): Promise<void> => {
        const response = await itemService.Add(CreateForm(addItem))
        if (response) {
            AllItems.value.push(response)
            fetchItems()
        }
    }
    const EditItem = async (editItem: IEditItem): Promise<void> => {
        const response = await itemService.Update(CreateForm(editItem))
        if (response) {
            const index = Items.value.findIndex((x) => x.id == editItem.id)
            AllItems.value.splice(index, 1, response)
            fetchItems()
        }
    }
    const fetchItems = () => {
        Items.value = AllItems.value
    }
    const DeleteItem = async (id: string): Promise<void> => {
        const response = await itemService.Delete(id)
        if (response) {
            useToster({ message: 'Item Deleted' })
            const index = Items.value.findIndex((x) => x.id == id)
            if (index != -1) {
                AllItems.value.splice(index, 1)
                fetchItems()
            }
        }
    }
    const AddComment = async (addCommnet: IAddComment): Promise<IComment | null> => {
        const comment = await itemService.PostComment(addCommnet)
        return comment
    }
    const updateStatus = async (itemId: string, status: ItemStatus): Promise<boolean> => {

        const res = await itemService.UpdateStatus(itemId, status)
        if (res) {
            const index = Items.value.findIndex(x => x.id == itemId)
            if (index != -1) {
                Items.value[index] = res
            }
            return true
        }
        return false
    }
    return {
        Items,
        KeepId,
        AddItem,
        EditItem,
        DeleteItem,
        GetAllItems,
        GetById,
        getSingalItem,
        AddComment,
        updateStatus
    }
})
export { ItemStore }
