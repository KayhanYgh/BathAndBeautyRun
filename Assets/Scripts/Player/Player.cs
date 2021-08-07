using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using Score;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public bool isPlayer = true;
        [ShowIf("@ !isPlayer")] public NpcDifficulty difficulty;
        public GameObject opponent;
        public bool attackSideIsLeft = true;

        [TabGroup("Setting")] public int health;
        [TabGroup("Setting")] public int heavyHitDamage;
        [TabGroup("Setting")] public int lightHitDamage;

        [TabGroup("Development")] public Animator animator;
        [TabGroup("Development")] public FixedJoystick fixedJoystick;
        [TabGroup("Development")] public Slider jumpForceSlider;
        [TabGroup("Development")] public float sliderSpeed;
        [TabGroup("Development")] public GameObject normalFace;
        [TabGroup("Development")] public GameObject deathFace;
        [TabGroup("Development")] public GameObject hitFace;
        [TabGroup("Development")] public Slider healthSlider;
        [TabGroup("Development")] public ParticleSystem heavyJumpEffect;
        [TabGroup("Development")] public ParticleSystem lightJumpEffect;
        [TabGroup("Development")] public ParticleSystem hitEffect1;
        [TabGroup("Development")] public ParticleSystem hitEffect2;
        [TabGroup("Development")] public GameObject[] takingHeavyHit;
        [TabGroup("Development")] public GameObject[] deathEmoji;
        [TabGroup("Development")] public GameObject[] heavyHitMessage;
        [TabGroup("Development")] public GameObject stunEffect;
        [TabGroup("Development")] public bool stun;
        [TabGroup("Development")] public GameObject winCamera;
        [TabGroup("Development")] public GameObject tears;
        [TabGroup("Development")] public GameObject[] balls;

        [TabGroup("Npc")] public bool stayStill;
        [TabGroup("Npc")] public bool attackIfPlayerIsNear;
        [TabGroup("Npc")] public bool dontGoRagDoll;
        [TabGroup("Npc")] public List<Transform> movePositions;
        [TabGroup("Npc")] public NavMeshAgent navMeshAgent;
        [TabGroup("Npc")] public int damage;
        [TabGroup("Npc")] public GameObject forceObject;
        [TabGroup("Npc")] public List<string> attackMotions;

        public WoodenPlank standingWoodenPlate;

        private bool _stop;

        private bool _reverseFlag;
        private static readonly int HeavyHit = Animator.StringToHash("HeavyHit");
        private static readonly int Movement = Animator.StringToHash("Movement");
        private static readonly int LightHit = Animator.StringToHash("LightHit");
        private bool _hitting;
        private static readonly int TakeHeavyHitDamage = Animator.StringToHash("TakeHeavyHitDamage");

        private int _chosenDeathMotion;
        private static readonly int ChosenDeathMotion = Animator.StringToHash("ChosenDeathMotion");
        private static readonly int Death1 = Animator.StringToHash("Death");
        public bool dontKillByOneHit;


        private void Start()
        {
            var chosenDeathMotionasd = Random.Range(0, 100) > 50 ? 1 : 2;
            animator.SetInteger(ChosenDeathMotion, chosenDeathMotionasd);

            if (!isPlayer)
            {
                int chosenMovement = Random.Range(0, 2);
                animator.SetInteger("RunMotion", chosenMovement);

                if (chosenMovement == 1 || chosenMovement == 2)
                {
                    navMeshAgent.speed = 3.5f;
                }
                else
                {
                    navMeshAgent.speed = 1f;
                }
            }
        }

        private void SliderHandler()
        {
            if (jumpForceSlider.value >= 100)
            {
                _reverseFlag = true;
            }

            if (jumpForceSlider.value <= 0)
            {
                _reverseFlag = false;
            }


            if (!_reverseFlag)
            {
                jumpForceSlider.value += sliderSpeed * Time.deltaTime;
            }
            else
            {
                jumpForceSlider.value -= sliderSpeed * Time.deltaTime;
            }
        }

        private void Update()
        {
            if (isPlayer)
            {
                if (health <= 0)
                {
                    Death();
                    ScoreManager.Instance.Lose("Player health is zero");
                }
            }

            if (ScoreManager.Instance.Won || ScoreManager.Instance.Lost || !ScoreManager.Instance.isStarted) return;

            if (_died)
                return;


            if (!_stop && !stun)
            {
                if (isPlayer)
                {
                    SliderHandler();
                    transform.position += Time.deltaTime * fixedJoystick.Horizontal * transform.right;
                    animator.SetInteger(Movement, Mathf.RoundToInt(fixedJoystick.Horizontal));
                }
                else
                {
                    //NpcMovement();
                    NewMovement();
                }
            }
        }

        private Transform _chosenMovePosition;

        private float _attackDelay;
        private bool _runnerAttackFlag;

        private bool _attackFlag;

        private void NewMovement()
        {
            if (attackIfPlayerIsNear)
            {
                if (Vector3.Distance(transform.position, RunnerPlayer.Instance.transform.position) < 0.5f)
                {
                    Debug.Log("asdasdasdasdsa");
                    if (!_died)
                        RunnerPlayer.Instance.Death();
                }
            }

            if (attackIfPlayerIsNear && !_runnerAttackFlag)
            {
                if (Vector3.Distance(transform.position, RunnerPlayer.Instance.transform.position) < 1.9f)
                {
                    animator.Play("attack_04");
                    _runnerAttackFlag = true;
                }
            }


            if (stun || stayStill) return;

            if (Vector3.Distance(transform.position, FpsPlayer.Instance.transform.position) < 1.8f)
            {
                animator.SetBool("Run", false);
                if (_attackDelay < Time.time)
                {
                    if (_attackFlag)
                    {
                        animator.Play(attackMotions[Random.Range(0, attackMotions.Count - 1)]);
                        FpsPlayer.Instance.TakeDamage(20, transform);
                    }

                    _attackFlag = true;
                    _attackDelay = Random.Range(1.5f, 2f) + Time.time;
                }
            }
            else
            {
                animator.SetBool("Run", true);
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(FpsPlayer.Instance.transform.position.x, transform.position.y,
                        FpsPlayer.Instance.transform.position.z),
                    navMeshAgent.speed * Time.deltaTime);

                transform.LookAt(new Vector3(FpsPlayer.Instance.transform.position.x, transform.position.y,
                    FpsPlayer.Instance.transform.position.z));
            }
        }

        private float _delay;

        private void NpcMovement()
        {
            if (stun) return;

            #region Movement

            if (!_chosenMovePosition) _chosenMovePosition = movePositions[Random.Range(0, movePositions.Count - 1)];

            if (Vector3.Distance(transform.position, _chosenMovePosition.position) < 0.5f)
                _chosenMovePosition = movePositions[Random.Range(0, movePositions.Count - 1)];

            transform.position = Vector3.MoveTowards(transform.position,
                _chosenMovePosition.position, 1 * Time.deltaTime);

            if (transform.position.z > _chosenMovePosition.position.z)
            {
                animator.SetInteger(Movement, 1);
            }
            else
            {
                animator.SetInteger(Movement, -1);
            }

            #endregion

            if (difficulty == NpcDifficulty.Easy)
            {
                if (_delay < Time.time)
                {
                    Jump();
                    _delay = Time.time + Random.Range(2, 3);
                }
            }
            else if (difficulty == NpcDifficulty.Normal)
            {
                if (_delay < Time.time)
                {
                    Jump();
                    _delay = Time.time + Random.Range(1.5f, 2.3f);
                }
            }
            else if (difficulty == NpcDifficulty.Hard)
            {
                if (_delay < Time.time)
                {
                    Jump();
                    _delay = Time.time + Random.Range(1, 2);
                }
            }
        }

        public void Jump()
        {
            if (stun) return;
            if (!standingWoodenPlate || _hitting) return;

            if (jumpForceSlider.value > 70)
            {
                if (Random.Range(0, 100) > 50)
                {
                    animator.SetBool(HeavyHit, true);
                }
                else
                {
                    animator.SetBool(LightHit, true);
                }

                MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
                StartCoroutine(nameof(HeavyHitProcess));
            }
            else
            {
                animator.SetBool(LightHit, true);
                MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
                StartCoroutine(nameof(LightHitProcess));
            }
        }

        private IEnumerator HeavyHitProcess()
        {
            _hitting = true;
            yield return new WaitForSeconds(0.8f);
            heavyJumpEffect.Play();
            try
            {
                standingWoodenPlate.HeavyHit(attackSideIsLeft);
            }
            catch
            {
                //ignore
            }

            animator.SetBool(HeavyHit, false);
            animator.SetBool(LightHit, false);

            yield return new WaitForSeconds(1.25f);
            _hitting = false;
        }

        private IEnumerator LightHitProcess()
        {
            _hitting = true;
            yield return new WaitForSeconds(0.55f);
            lightJumpEffect.Play();

            try
            {
                standingWoodenPlate.LightHit(attackSideIsLeft);
            }
            catch
            {
                //ignore
            }

            animator.SetBool(LightHit, false);

            yield return new WaitForSeconds(1.25f);
            _hitting = false;
        }

        public void TakeHeavyHit()
        {
            StartCoroutine(nameof(TakingHeavyHitDamageProcess));
        }

        private IEnumerator TakingHeavyHitDamageProcess()
        {
            if (!isPlayer)
            {
                Instantiate(heavyHitMessage[Random.Range(0, 2)]);
            }


            if (isPlayer)
            {
                health -= heavyHitDamage / 3;
            }
            else
            {
                health -= heavyHitDamage;
            }

            healthSlider.value = health;
            hitEffect1.gameObject.SetActive(true);
            hitEffect2.gameObject.SetActive(true);
            var chosenHitEffect = takingHeavyHit[Random.Range(0, takingHeavyHit.Length - 1)];
            chosenHitEffect.SetActive(true);
            Death();
            yield return new WaitForSeconds(0);
            _stop = true;
            animator.SetBool(TakeHeavyHitDamage, true);
            hitFace.SetActive(true);
            normalFace.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _stop = false;
            hitFace.SetActive(false);
            normalFace.SetActive(true);
            animator.SetBool(TakeHeavyHitDamage, false);
            yield return new WaitForSeconds(0);

            yield return new WaitForSeconds(0.5f);

            chosenHitEffect.SetActive(false);
            hitEffect1.gameObject.SetActive(false);
            hitEffect2.gameObject.SetActive(false);
        }

        public void TakeHeavyHit_Stun()
        {
            StartCoroutine(nameof(TakingHeavyHitStunDamageProcess));
        }

        private IEnumerator TakingHeavyHitStunDamageProcess()
        {
            if (!isPlayer)
            {
                Instantiate(heavyHitMessage[Random.Range(0, 2)]);
            }

            stun = true;
            stunEffect.SetActive(true);
            hitEffect1.gameObject.SetActive(true);
            hitEffect2.gameObject.SetActive(true);

            Death();
            yield return new WaitForSeconds(0);
            _stop = true;
            animator.SetBool(TakeHeavyHitDamage, true);
            hitFace.SetActive(true);
            normalFace.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _stop = false;
            hitFace.SetActive(false);
            normalFace.SetActive(true);
            animator.SetBool(TakeHeavyHitDamage, false);
            yield return new WaitForSeconds(0);

            yield return new WaitForSeconds(0.5f);

            hitEffect1.gameObject.SetActive(false);
            hitEffect2.gameObject.SetActive(false);

            yield return new WaitForSeconds(2);
            stun = false;
            stunEffect.SetActive(false);

            yield return new WaitForSeconds(0);

            yield break;
        }

        public void TakeLightHit()
        {
            StartCoroutine(nameof(TakingLightHitDamageProcess));
        }

        private IEnumerator TakingLightHitDamageProcess()
        {
            if (isPlayer)
            {
                health -= lightHitDamage / 3;
            }
            else
            {
                health -= lightHitDamage;
            }

            healthSlider.value = health;
            Death();
            yield return new WaitForSeconds(0);
            _stop = true;
            animator.SetBool(TakeHeavyHitDamage, true);
            hitFace.SetActive(true);
            normalFace.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _stop = false;
            hitFace.SetActive(false);
            normalFace.SetActive(true);
            animator.SetBool(TakeHeavyHitDamage, false);
            yield return new WaitForSeconds(0);
        }


        public void TakeLightHit_DoubleDamage()
        {
            StartCoroutine(nameof(TakingLightHitDoubleDamageProcess));
        }

        private IEnumerator TakingLightHitDoubleDamageProcess()
        {
            health -= lightHitDamage * 2;
            healthSlider.value = health;

            Death();
            yield return new WaitForSeconds(0);
            _stop = true;
            animator.SetBool(TakeHeavyHitDamage, true);
            hitFace.SetActive(true);
            tears?.SetActive(true);
            normalFace.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _stop = false;
            hitFace.SetActive(false);
            normalFace.SetActive(true);
            tears?.SetActive(false);
            animator.SetBool(TakeHeavyHitDamage, false);
            yield return new WaitForSeconds(0);
        }

        [HideInInspector] public bool _died;
        private static readonly int Dance1 = Animator.StringToHash("Dance");
        private static readonly int ChosenDance = Animator.StringToHash("ChosenDance");

        public UnityEvent onDeath;

        private void Death()
        {
            if (_died)
                return;

            if (stayStill) ResetStamina();

            if (!isPlayer)
                deathEmoji[Random.Range(0, deathEmoji.Length)].SetActive(true);

            if (health <= 0)
            {
                _stop = true;
                _died = true;
                animator.SetBool(Death1, true);
                hitFace.SetActive(false);
                normalFace.GetComponent<SkinnedMeshRenderer>().enabled = false;
                normalFace.SetActive(false);
                deathFace.SetActive(true);

                if (!isPlayer)
                    tears?.SetActive(true);
                if (isPlayer)
                {
//                    ScoreManager.Instance.Lose("");
                    foreach (var item in ScoreManager.Instance.npcs)
                    {
                        item.Dance();
                    }
                }
//                else
//                {
//                    ScoreManager.Instance.Win("");
//                    StartCoroutine(nameof(SlowMotionProcess));
//                }

                foreach (var item in balls)
                {
                    item.SetActive(true);
                }

                if (!dontGoRagDoll)
                {
                    if (Random.Range(0, 100) > 40)
                    {
                        Invoke(nameof(GoRagDoll), Random.Range(2.3f, 3f));
                    }
                    else
                    {
                        GoRagDoll();
                        forceObject.SetActive(true);
                    }
                }


                // opponent.GetComponent<Player>().Dance();
            }

            onDeath?.Invoke();
        }


        private void GoRagDoll()
        {
            animator.enabled = false;
        }

        public void SlowMotion()
        {
            StartCoroutine(nameof(SlowMotion));
        }

        private IEnumerator SlowMotionProcess()
        {
            Time.timeScale = 0.7f;

            yield return new WaitForSecondsRealtime(2);

            Time.timeScale = 0.3f;
            Time.timeScale = 0.3f;

            yield return new WaitForSecondsRealtime(2.5f);

            Time.timeScale = 1f;

            if (winCamera)
                winCamera.SetActive(true);

            yield return new WaitForSecondsRealtime(2.5f);
            // animator.enabled = false;
        }

        public void Dance()
        {
            animator.SetInteger(ChosenDance, Random.Range(1, 4));
            animator.SetBool(Dance1, true);
        }


        public enum NpcDifficulty
        {
            Easy,
            Normal,
            Hard
        }


        public void ResetStamina()
        {
            RunnerPlayer.Instance.stamina = 1;
        }
    }
}