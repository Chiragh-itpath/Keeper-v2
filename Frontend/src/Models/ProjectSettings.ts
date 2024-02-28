interface IProjectSetting {
    statusList: IStatus[]
    clientList: IClient[]
}

interface IStatus {
    id: string
    projectId: string
    title: string,
    isSystem: boolean
}

interface IClient {
    id: string
    projectId: string
    name: string
}
interface RuleBook {
    projectId: string,
    text: string
}

type EditStatus = Omit<IStatus, 'isSystem'>
type AddStatus = Omit<EditStatus, 'id'>
type EditClient = IClient
type AddClient = Omit<EditClient, 'id'>

export type {
    IProjectSetting,
    IStatus,
    IClient,
    RuleBook,
    EditStatus,
    AddStatus,
    EditClient,
    AddClient
}