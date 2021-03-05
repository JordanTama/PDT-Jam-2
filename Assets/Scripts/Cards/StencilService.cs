using ServiceLocator;

namespace Cards
{
    public class StencilService : IService
    {
        private int _stencilRef;
        
        private const int Low = 1;
        private const int High = 255;
        
        public StencilService()
        {
            _stencilRef = Low;
        }

        public int GetStencilRef()
        {
            int value = _stencilRef;
            Increment();
            
            return value;
        }

        private void Increment()
        {
            _stencilRef += 1;
            
            if (_stencilRef > High)
                _stencilRef = Low;
        }
    }
}