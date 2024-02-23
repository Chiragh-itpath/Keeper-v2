interface IProjectSetting {
    statusList: Status[]
    clientList: Client[]
}

interface Status {
    id: string
    projectId: string
    title: string
}

interface Client {
    id: string
    projectId: string
    fullname: string
}

export type {
    IProjectSetting,
    Status,
    Client
}