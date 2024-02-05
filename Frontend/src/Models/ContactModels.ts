interface IContact extends IAddContact {
    id: string,    
}
interface IAddContact {
    firstName: string,
    lastName: string,
    email: string
}
export type { IContact, IAddContact }
