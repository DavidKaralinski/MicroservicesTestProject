import { SiStudyverse } from 'react-icons/si'

export const NavBar = () => {
    return (
        <header className='sticky top-0 z-50 flex justify-between shadow-md bg-white items-center p-5'>
            <div className='flex items-center gap-2 text-3xl font-semibold text-gray-600'>
                <SiStudyverse size={34} />
                <div>Test Project</div>
            </div>
            <div className='text-center'>Search</div>
            <div>Login</div>
        </header>
    )
}